/**
 * DevExtreme (integration/knockout/clean_node_old.js)
 * Version: 19.1.5
 * Build date: Tue Jul 30 2019
 *
 * Copyright (c) 2012 - 2019 Developer Express Inc. ALL RIGHTS RESERVED
 * Read about DevExtreme licensing here: https://js.devexpress.com/Licensing/
 */
"use strict";
var ko = require("knockout"),
    compareVersion = require("../../core/utils/version").compare,
    strategyChanging = require("../../core/element_data").strategyChanging;
var patchCleanData = function(jQuery) {
    var cleanKoData = function(element, andSelf) {
        var cleanNode = function() {
            ko.cleanNode(this)
        };
        if (andSelf) {
            element.each(cleanNode)
        } else {
            element.find("*").each(cleanNode)
        }
    };
    var originalEmpty = jQuery.fn.empty;
    jQuery.fn.empty = function() {
        cleanKoData(this, false);
        return originalEmpty.apply(this, arguments)
    };
    var originalRemove = jQuery.fn.remove;
    jQuery.fn.remove = function(selector, keepData) {
        if (!keepData) {
            var subject = this;
            if (selector) {
                subject = subject.filter(selector)
            }
            cleanKoData(subject, true)
        }
        return originalRemove.call(this, selector, keepData)
    };
    var originalHtml = jQuery.fn.html;
    jQuery.fn.html = function(value) {
        if ("string" === typeof value) {
            cleanKoData(this, false)
        }
        return originalHtml.apply(this, arguments)
    };
    var originalReplaceWith = jQuery.fn.replaceWith;
    jQuery.fn.replaceWith = function() {
        var result = originalReplaceWith.apply(this, arguments);
        if (!this.parent().length) {
            cleanKoData(this, true)
        }
        return result
    }
};
strategyChanging.add(function(strategy) {
    var isJQuery = !!strategy.fn;
    if (isJQuery && compareVersion(strategy.fn.jquery, [2, 0]) < 0) {
        patchCleanData(strategy)
    }
});
