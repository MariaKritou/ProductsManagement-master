




$("body").on("click", "#btnBuy", function () {
  console.log("on click");
  //Loop through the Table rows and build a JSON array.
  var products = new Array();
  $("#tblProducts TBODY TR").each(function () {
    console.log("on click 2");
    var row = $(this);
    var product = {};
    product.productId = row.find("TD").eq(0).html();
    //product.description = row.find("TD").eq(1).html();
    //product.price = row.find("TD").eq(2).html();
    product.quantity = row.find("TD").eq(3).html();
    products.push(product);
  });
  console.log(products);

  //Send the JSON array to Controller using AJAX.
  $.ajax({
    type: "POST",
    url: "/Cart/BuyAll",
    data: JSON.stringify(products),
    contentType: "application/json; charset=utf-8",
    dataType: "json",
    success: function (r) {
      alert(r + " products inserted.");
    }
  });
  console.log(JSON.stringify(products));
});