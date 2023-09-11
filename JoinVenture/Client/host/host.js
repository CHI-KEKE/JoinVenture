if (!accessToken) {
  openPopup();
}


function isValidPositiveNumber(value) {
  var positiveIntegerPattern = /^\d+$/;
  return positiveIntegerPattern.test(value) && parseInt(value) > 0;
}


// Handle the form submission
$(".submit_btn").click(function () {
  var ticketPackages = [];
  $(".error-message").empty();

  //Checking ticket Price should be Positive!
  var priceInputs = $(".ticketPackage-price");
  var allInputsValid = true;

  priceInputs.each(function () {
    var inputValue = $(this).val();

    if (!isValidPositiveNumber(inputValue)) {
      $(this).next(".error-message").text("請符合實際使用考量");
      allInputsValid = false; // Mark that at least one input is invalid
    }
  });


  //Checking ticket count should be Positive!
  var countInputs = $(".ticketPackage-count");

  countInputs.each(function () {
    var inputValue = $(this).val();

    if (!isValidPositiveNumber(inputValue)) {
      $(this).next(".error-message").text("請符合實際使用考量");
      allInputsValid = false; // Mark that at least one input is invalid
    }
  });



  //Checking no empty

  var allInputs = $("form :input");
  allInputs.each(function () {
    var inputValue = $(this).val();

    // Check if the input is empty
    if (!inputValue.trim()) {
      // Display an error message under the input
      $(this).next(".error-message").text("請填答");
      allInputsValid = false; // Mark that at least one required input is invalid
    }
  });


  //summary prevent or not
  if (!allInputsValid) {
    event.preventDefault();
  }



  // Iterate through the ticket package form groups
  $(".ticketPackage").each(function () {
    var ticketPackage = {
      title: $(this).find(".ticketPackage-title").val(),
      price: parseFloat($(this).find(".ticketPackage-price").val()),
      description: $(this).find(".ticketPackage-description").val(),
      count: parseInt($(this).find(".ticketPackage-count").val()),
      validatedDateStart: $(this)
        .find(".ticketPackage-validateDateStart input")
        .val(),
      validatedDateEnd: $(this)
        .find(".ticketPackage-validateDateEnd input")
        .val(),
      bookingAvailableStart: $(this)
        .find(".ticketPackage-bookingAvailibleStart input")
        .val(),
      bookingAvailableEnd: $(this)
        .find(".ticketPackage-bookingAvailibleEnd input")
        .val(),
    };

    ticketPackages.push(ticketPackage);
  });

  var ticketPackagesJson = JSON.stringify(ticketPackages);
  const formData = new FormData();
  formData.append("title", $("#Title").val());
  formData.append("date", $("#dateOfActivity").val());
  formData.append("description", $("#Description").val());
  formData.append("category", $("#Category").val());
  formData.append("city", $("#city").val());
  formData.append("venue", $("#venue").val());
  formData.append("image", $("#image")[0].files[0]);
  formData.append("ticketPackages", ticketPackagesJson);

  $.post({
    url: `${baseUrl}Activities`,
    contentType: false,
    data: formData,
    processData: false,
    beforeSend: function (xhr) {
      xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
    },
    success: (res) => {
      toastr["success"]("", "活動建立成功!");
      setTimeout(window.location.href = "https://cofstyle.shop/list/Activity-List.html",2000);
      
    },
    error: (err) => {
      console.log(err);
    },
  });
});
