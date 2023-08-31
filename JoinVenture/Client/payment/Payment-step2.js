/////////////////////////////////////////////////////////////////////////////////////Date Transfer

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
  ///////////////////////////////////////////////////////////////////////////////////////Activity Theme

  const queryParams = new URLSearchParams(window.location.search);
  const activityIdFromURL = queryParams.get("id");

  axios
    .get(`${baseUrl}Activities/${activityIdFromURL}`)
    .then(function (response) {
      const activity = response.data;
      console.log("For theme" + response.data);

      const formattedDate = formatDate(activity.date);

      $(".activity-title").text(activity.title);
      $(".activity-validatedate").text(formattedDate);
      $(".activity-location").text(`${activity.city}, ${activity.venue}`);
      $(".activity-image img").attr("src", activity.image);
    });

  ///////////////////////////////////////////////////////////////////////////////////////Stepper Settings

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

  $("div.setup-panel div a.btn-primary").trigger("click");

  //////////////////////////////////////////////////////////////////////////////////////Timer/////////////////////////////////////////

  function format(time) {
    // terrinary operator same as IF-else
    return time < 10 ? `0${time}` : time;
  }

  let totalseconds = 600;

  function updateTime() {
    console.log("called me!");

    totalseconds = --totalseconds;

    const totalmins = Math.floor(totalseconds / 60);
    // Total Secounds remaining till the event Excluding the upper days
    const totalsecs = Math.floor(totalseconds) % 60;

    document.getElementById("min").innerHTML = format(totalmins);
    document.getElementById("sec").innerHTML = format(totalsecs);

    if (totalmins == "00" && totalsecs == "00") {
      // Calls the animated funtion to show the animation
      window.location.href = `https://fff5-2402-7500-4d5-a113-e930-21d7-3d9c-cf18.ngrok-free.app/Client/ticket-selecting/Ticket-Selecting.html?id=${activityIdFromURL}`;
    }
  }

  setInterval(updateTime, 1000);
});

///////////////////////////////////////////////////////////////////////////////////////Step 1 terms must be checked !! + Next Button logic

const agreeCheckbox = document.getElementById("MustBeChecked");
const nextButton = document.getElementById("nextButton");

// Add a click event listener to the Next button
nextButton.addEventListener("click", function (e) {
  // Check if the checkbox is checked
  if (!agreeCheckbox.checked) {
    alert("Please agree to the terms and conditions.");
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

    (allNextBtn = $(".nextBtn")),
      allNextBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
          curStepBtn = curStep.attr("id"),
          nextStepWizard = $(
            'div.setup-panel div a[href="#' + curStepBtn + '"]'
          )
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
