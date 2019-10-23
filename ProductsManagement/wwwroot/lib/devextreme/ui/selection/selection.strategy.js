/**
 * DevExtreme (ui/selection/selection.strategy.js)
 * Version: 19.1.5
 * Build date: Tue Jul 30 2019
 *
 * Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
 * Read about DevExtreme licensing here: https://js.devexpress.com/Licensing/
 */
"use strict";
var dataQuery = require("../../data/query"),
    commonUtils = require("../../core/utils/common"),
    typeUtils = require("../../core/utils/type"),
    getKeyHash = commonUtils.getKeyHash,
    Class = require("../../core/class"),
    Deferred = require("../../core/utils/deferred").Deferred;
module.exports = Class.inherit({
    ctor: function(options) {
        this.options = options;
        this._clearItemKeys()
    },
    _clearItemKeys: function() {
        this._setOption("addedItemKeys", []);
        this._setOption("removedItemKeys", []);
        this._setOption("removedItems", []);
        this._setOption("addedItems", [])
    },
    validate: commonUtils.noop,
    _setOption: function(name, value) {
        this.options[name] = value
    },
    onSelectionChanged: function onSelectionChanged() {
        var addedItemKeys = this.options.addedItemKeys,
            removedItemKeys = this.options.removedItemKeys,
            addedItems = this.options.addedItems,
            removedItems = this.options.removedItems,
            selectedItems = this.options.selectedItems,
            selectedItemKeys = this.options.selectedItemKeys,
            onSelectionChanged = this.options.onSelectionChanged || commonUtils.noop;
        this._clearItemKeys();
        onSelectionChanged({
            selectedItems: selectedItems,
            selectedItemKeys: selectedItemKeys,
            addedItemKeys: addedItemKeys,
            removedItemKeys: removedItemKeys,
            addedItems: addedItems,
            removedItems: removedItems
        })
    },
    equalKeys: function(key1, key2) {
        if (this.options.equalByReference) {
            if (typeUtils.isObject(key1) && typeUtils.isObject(key2)) {
                return key1 === key2
            }
        }
        return commonUtils.equalByValue(key1, key2)
    },
    _clearSelection: function(keys, preserve, isDeselect, isSelectAll) {
        keys = keys || [];
        keys = Array.isArray(keys) ? keys : [keys];
        this.validate();
        return this.selectedItemKeys(keys, preserve, isDeselect, isSelectAll)
    },
    _loadFilteredData: function(remoteFilter, localFilter, select) {
        var filterLength = encodeURI(JSON.stringify(remoteFilter)).length,
            needLoadAllData = this.options.maxFilterLengthInRequest && filterLength > this.options.maxFilterLengthInRequest,
            deferred = new Deferred,
            loadOptions = {
                filter: needLoadAllData ? void 0 : remoteFilter,
                select: needLoadAllData ? this.options.dataFields() : select || this.options.dataFields()
            };
        if (remoteFilter && 0 === remoteFilter.length) {
            deferred.resolve([])
        } else {
            this.options.load(loadOptions).done(function(items) {
                var filteredItems = typeUtils.isPlainObject(items) ? items.data : items;
                if (localFilter) {
                    filteredItems = filteredItems.filter(localFilter)
                } else {
                    if (needLoadAllData) {
                        filteredItems = dataQuery(filteredItems).filter(remoteFilter).toArray()
                    }
                }
                deferred.resolve(filteredItems)
            }).fail(deferred.reject.bind(deferred))
        }
        return deferred
    },
    updateSelectedItemKeyHash: function(keys) {
        for (var i = 0; i < keys.length; i++) {
            var keyHash = getKeyHash(keys[i]);
            if (!typeUtils.isObject(keyHash)) {
                this.options.keyHashIndices[keyHash] = this.options.keyHashIndices[keyHash] || [];
                var keyIndices = this.options.keyHashIndices[keyHash];
                keyIndices.push(i)
            }
        }
    },
    _isAnyItemSelected: function(items) {
        for (var i = 0; i < items.length; i++) {
            if (this.options.isItemSelected(items[i])) {
                return
            }
        }
        return false
    },
    _getFullSelectAllState: function() {
        var items = this.options.plainItems(),
            dataFilter = this.options.filter(),
            selectedItems = this.options.selectedItems;
        if (dataFilter) {
            selectedItems = dataQuery(selectedItems).filter(dataFilter).toArray()
        }
        var selectedItemsLength = selectedItems.length;
        if (!selectedItemsLength) {
            return this._isAnyItemSelected(items)
        }
        if (selectedItemsLength >= this.options.totalCount()) {
            return true
        }
        return
    },
    _getVisibleSelectAllState: function() {
        var items = this.options.plainItems(),
            hasSelectedItems = false,
            hasUnselectedItems = false;
        for (var i = 0; i < items.length; i++) {
            var item = items[i],
                itemData = this.options.getItemData(item),
                key = this.options.keyOf(itemData);
            if (this.options.isSelectableItem(item)) {
                if (this.isItemKeySelected(key)) {
                    hasSelectedItems = true
                } else {
                    hasUnselectedItems = true
                }
            }
        }
        if (hasSelectedItems) {
            return !hasUnselectedItems ? true : void 0
        } else {
            return false
        }
    }
});
