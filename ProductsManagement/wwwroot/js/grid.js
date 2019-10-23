
connection.on("OnProductCreated", function () {
  var dataGrid = $("#gridContainer").dxDataGrid("instance");
  dataGrid.refresh();
});