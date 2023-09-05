if (!accessToken) {
  openPopup();
}


// Handle the form submission
$(".submit_btn").click(function () {
  // Prepare the data for the POST request

  var ticketPackages = [];

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
      console.log(res);

      window.location.href = "https://cofstyle.shop/list/Activity-List.html";
    },
    error: (err) => {
      console.log(err);
    },
  });
});
