//Date Transfer

function formatDate(dateString) {
  const date = new Date(dateString);
  const year = date.getFullYear();
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const day = date.getDate().toString().padStart(2, "0");
  const hours = date.getHours().toString().padStart(2, "0");
  const minutes = date.getMinutes().toString().padStart(2, "0");
  return `${year}-${month}-${day} ${hours}:${minutes}`;
}



$(document).ready(function () {

  //Activity Theme

  const queryParams = new URLSearchParams(window.location.search);
  const activityIdFromURL = queryParams.get("id");

  axios
    .get(`http://localhost:5000/api/Activities/${activityIdFromURL}`)
    .then(function (response) {
      const activity = response.data;
      console.log("For theme" + response.data);

      const formattedDate = formatDate(activity.date);

      $(".activity-title").text(activity.title);
      $(".activity-validatedate").text(formattedDate);
      $(".activity-location").text(`${activity.city}, ${activity.venue}`);
      $(".activity-image img").attr("src", activity.image);
    });
  


  //Stepper Settings

  var navListItems = $("div.setup-panel div a"),
    allWells = $(".setup-content"),
    // allNextBtn = $(".nextBtn"),
    allPrevBtn = $(".prevBtn");

  allWells.hide();

  navListItems.click(function (e) {
    e.preventDefault();
    var $target = $($(this).attr("href")),
      $item = $(this);

    if (!$item.hasClass("disabled")) {
      navListItems.removeClass("btn-primary").addClass("btn-default");
      $item.addClass("btn-primary");
      allWells.hide();
      $target.show();
      $target.find("input:eq(0)").focus();
    }
  });

  allPrevBtn.click(function () {
    var curStep = $(this).closest(".setup-content"),
      curStepBtn = curStep.attr("id"),
      prevStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]')
        .parent()
        .prev()
        .children("a");

    prevStepWizard.removeAttr("disabled").trigger("click");
  });

  // allNextBtn.click(function () {
  //   var curStep = $(this).closest(".setup-content"),
  //     curStepBtn = curStep.attr("id"),
  //     nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]')
  //       .parent()
  //       .next()
  //       .children("a"),
  //     curInputs = curStep.find("input[type='text'],input[type='url']"),
  //     isValid = true;

  //   $(".form-group").removeClass("has-error");
  //   for (var i = 0; i < curInputs.length; i++) {
  //     if (!curInputs[i].validity.valid) {
  //       isValid = false;
  //       $(curInputs[i]).closest(".form-group").addClass("has-error");
  //     }
  //   }

  //   if (isValid) nextStepWizard.removeAttr("disabled").trigger("click");
  // });

  $("div.setup-panel div a.btn-primary").trigger("click");

  //Step3 Page Info

  //Price Info
  // let totalPrice = localStorage.getItem("totalPrice");
  // $(".finalTotalPrice").text("NT$ " + totalPrice);

  //For Order Info

  // var listOfActivityPackagesStr = localStorage.getItem("selectedTickets");
  // var listOfActivityPackages = JSON.parse(listOfActivityPackagesStr);
  // var activityId = listOfActivityPackages[0].activityId;
  // var jwtToken = localStorage.getItem("token");

  // $.get({
  //   url: `http://localhost:5000/api/Order/${activityId}`,
  //   dataType: "json",
  //   contentType: "application/json",
  //   beforeSend: function (xhr) {
      // Set the Authorization header with the JWT token
  //     xhr.setRequestHeader("Authorization", "Bearer " + jwtToken);
  //   },
  //   success: (res) => {
  //     $(".invoiceNumber").text("Invoice Number : " + res.id);
  //     var date = new Date(res.invoiceDate);
  //     var formattedDate =
  //       date.getFullYear() +
  //       "-" +
  //       ("0" + (date.getMonth() + 1)).slice(-2) +
  //       "-" +
  //       ("0" + date.getDate()).slice(-2) +
  //       " " +
  //       ("0" + date.getHours()).slice(-2) +
  //       ":" +
  //       ("0" + date.getMinutes()).slice(-2) +
  //       ":" +
  //       ("0" + date.getSeconds()).slice(-2);

  //     $(".invoiceDate").text("Invoice Date: " + formattedDate);
  //   },
  //   error: (err) => {
  //     console.log(err);
  //   },
  // });

  //for TicketPackages  card Info
  // var PackageContainer = document.querySelector(".packageCardsContainer");

  // listOfActivityPackages.forEach(function (package){

  //   var packageDiv = document.createElement("div");
  //   packageDiv.className = "row mt-4";
  //   packageDiv.innerHTML = `
  //         <div class="col">
  //             <div class="card card-2">
  //                 <div class="card-body">
  //                     <div class="media">
  //                         <div class="sq align-self-center"> <img class="img-fluid  my-auto align-self-center mr-2 mr-md-4 pl-0 p-0 m-0" src="https://t4.ftcdn.net/jpg/04/77/44/03/240_F_477440329_x4AOe1grWqyOQu335eqsifroDiVefft0.jpg" width="135" height="135" /> </div>
  //                         <div class="media-body my-auto text-right">
  //                             <div class="row  my-auto flex-column flex-md-row">
  //                               <div class="d-flex flex-column">
  //                                 <div class="col-auto mt-3"> <h6 class="mb-0">票種 : ${package.title}</h6> </div>
  //                                 <div class="col mt-2"> <small>活動場地: ${package.venue}</small></div>
  //                                 <div class="col mt-2 "> <small>活動時間: ${package.validDate}</small></div>
  //                               </div>
  //                               <div class = "d-flex justify-content-between mt-2">
  //                                 <div class="col my-auto  "> <small>票數 : ${package.quantity}</small></div>
  //                                 <div class="my-auto">  <h6 class="mb-0">NT$ ${package.price}</h6></div>
  //                               </div>
  //                             </div>
  //                         </div>
  //                     </div>
  //                 </div>
  //             </div>
  //         </div>
  //   `;

    // Append the packageDiv to the container
  //   PackageContainer.insertAdjacentElement("afterend", packageDiv);

  // })

});


//Step 1 terms must be checked !!

    const agreeCheckbox = document.getElementById("MustBeChecked");
    const nextButton = document.getElementById('nextButton');

    // Add a click event listener to the Next button
    nextButton.addEventListener('click', function (e) {
        // Check if the checkbox is checked
        if (!agreeCheckbox.checked) {
            alert('Please agree to the terms and conditions.');
            e.preventDefault();

        } else {

            const name = document.querySelector("#name").value;
            const email = document.querySelector("#email").value;
            const phoneNumber = document.querySelector("#phoneNumber").value;
            const dateOfBirth = document.querySelector("#dateOfBirth").value;

            // Get the selected value of the radio buttons
            const selectedOption = document.querySelector(
              "input[name='optionsRadios']:checked"
            ).value;

            // Create an object with the collected data
            const userInfoData = {
              name: name,
              email: email,
              phoneNumber: phoneNumber,
              dateOfBirth: dateOfBirth,
              selectedOption: selectedOption,
            };

            // Store the serialized form data in local storage
            localStorage.setItem("step1FormData", JSON.stringify(userInfoData));

            allNextBtn = $(".nextBtn"),

            allNextBtn.click(function () {
              var curStep = $(this).closest(".setup-content"),
                curStepBtn = curStep.attr("id"),
                nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]')
                  .parent()
                  .next()
                  .children("a"),
                curInputs = curStep.find("input[type='text'],input[type='url']"),
                isValid = true;

              $(".form-group").removeClass("has-error");
              for (var i = 0; i < curInputs.length; i++) {
                if (!curInputs[i].validity.valid) {
                  isValid = false;
                  $(curInputs[i]).closest(".form-group").addClass("has-error");
                }
              }

              if (isValid) nextStepWizard.removeAttr("disabled").trigger("click");
            });

        }
    });