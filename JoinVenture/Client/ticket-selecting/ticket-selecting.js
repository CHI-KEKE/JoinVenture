const queryParams = new URLSearchParams(window.location.search);
const activityId = queryParams.get("id");


const selectedTickets = [];

//Packages////////////////////////////////////////////////////////////////

function countAvailableTickets(package) {
  return package.tickets.filter((ticket) => ticket.status === "Available")
    .length;
}

function createPackage(package, index, availableCount) {
  const isHidden = availableCount === 0;
  const buttonDisplay = isHidden ? "none" : "block"; // Conditionally set display property

  return `
            <div class="accordion-item">
                <div class="accordion-header">
                    <div class="accordion-title">
                        <h5 class="p-3">${package.title}</h5>
                        <h5 class="ps-3">NT$ ${package.price}</h5>
                        <h6 id="ticketCount-${
                          package.title
                        }" class ="ps-3 remainCount" style = "color:#DBBEA1;">剩餘票數 : ${availableCount} 張</h6>
                    </div>
                    <div class="buttons">
                        <button id = "addButton-${
                          package.title
                        }" class="subbutton add" style="display: ${buttonDisplay};" >+</button>
                        <button id = "minusButton-${
                          package.title
                        }"class="subbutton minus" style="display: ${buttonDisplay};">-</button>
                    </div>
                </div>
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#${index}" aria-expanded="false" aria-controls="${index}">
                    More Info
                </button>
                </h2>
                <div id="${index}" class="accordion-collapse collapse" data-bs-parent="#accordionExample">
                <div class="accordion-body">
                    <strong>Booking Available</strong> 
                    <p>${formatDate(
                      package.bookingAvailableStart
                    )} - ${formatDate(package.bookingAvailableEnd)}</p>
                    <br>
                    <strong>Valid Date</strong> 
                    <p>${formatDate(package.validatedDateStart)} - ${formatDate(
    package.validatedDateEnd
  )}</p>
                    <br>
                    <strong>Ticket Description</strong> 
                    <p>${package.description}</p>
                    <br>
                </div>
                </div>
            </div>
            </div>
        `;
}

//Packages/////////////////////////////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////////////////////////////left hand side ActivityInfo//////////////

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

  if (accessToken == null) {
    console.log("I am at ticketselecting.js!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

    openPopup();
  }

  localStorage.removeItem("selectedTickets");
  localStorage.removeItem("totalPrice");
  localStorage.removeItem("step1FormData");
  localStorage.removeItem("ticketInfo");
  localStorage.removeItem("orderId");

  axios
    .get(`${baseUrl}Activities/${activityId}`)
    .then(function (response) {
      //the overall data
      const activity = response.data;
      const activityPackages = response.data.ticketPackages;
      console.log(response.data);

      //for datetime format
      const formattedDate = formatDate(activity.date);

      //to set the left side activity info
      $(".activity-title").text(activity.title);
      $(".activity-validatedate").text(formattedDate);
      $(".activity-location").text(`${activity.city}, ${activity.venue}`);
      $(".activity-image img").attr("src", activity.image);

      // create Cards for each Package
      activityPackages.forEach(function (package, index) {
        const availableCount = countAvailableTickets(package);
        const newPackageHTML = createPackage(package, index, availableCount);

        if (availableCount === 0) {
        }
        // Append new package to the appropriate place
        $(".accordion").append(newPackageHTML);
      });

      ///////////////////////////////////////////////////////////////////////////////////left hand side ActivityInfo//////////////

      ///////////////////////////////////////////////////////////////////////////////////Add the functionality for the add/minus buttons
      const addButton = document.querySelectorAll(".subbutton.add");
      const minusButton = document.querySelectorAll(".subbutton.minus");
      const totalTickets = document.querySelector(".total-tickets");
      const totalPrice = document.querySelector(".total-price");

      let ticketCounts = {};
      let ticketPrices = {};
      let totalTicketPrice = 0;

      //Add Button............................................................................................................................

      addButton.forEach((button, index) => {
        //Identify which Add Button
        const ticketType = index;

        //Grab the coressponing price
        const ticketPriceText = button
          .closest(".accordion-item")
          .querySelector(".ps-3").textContent;

        const ticketPrice = parseInt(ticketPriceText.replace(/[^0-9]/g, ""));

        //Grab the corressponding remain Tickets
        const ticketRemainText = button
          .closest(".accordion-item")
          .querySelector(".remainCount").textContent;

        const ticketRemainCount = parseInt(
          ticketRemainText.replace(/[^0-9]/g, "")
        );

        //set initial count and price
        ticketCounts[ticketType] = 0;
        ticketPrices[ticketType] = ticketPrice;

        //when clicked
        button.addEventListener("click", function () {
          console.log(activityPackages[ticketType].id);
          console.log(ticketPrice);

          //Remain ticket should >= user slection uplimit

          if (ticketCounts[ticketType] < ticketRemainCount) {
            ticketCounts[ticketType]++;
            totalTicketPrice += ticketPrice;

            updateTotals();

            //For LocalStorage

            //First check if the ticket exists
            const selectedTicketIndex = selectedTickets.findIndex((ticket) => {
              return (
                ticket.activityId === activityId &&
                ticket.activityImage === activity.image &&
                ticket.venue === activity.venue &&
                ticket.activityTitle === activity.title &&
                ticket.title === activityPackages[ticketType].title &&
                ticket.price === activityPackages[ticketType].price &&
                ticket.bookingAvailability ===
                  `${formatDate(
                    activityPackages[ticketType].bookingAvailableStart
                  )} - ${formatDate(
                    activityPackages[ticketType].bookingAvailableEnd
                  )}` &&
                ticket.validDate ===
                  `${formatDate(
                    activityPackages[ticketType].validatedDateStart
                  )} - ${formatDate(
                    activityPackages[ticketType].validatedDateEnd
                  )}` &&
                ticket.description === activityPackages[ticketType].description
              );
            });

            if (selectedTicketIndex !== -1) {
              // Ticket already exists, increment the quantity
              selectedTickets[selectedTicketIndex].quantity += 1;
            } else {
              // Ticket doesn't exist, add it to the array

              const selectedTicket = {
                activityId: activityId,
                activityImage: activity.image,
                venue: activity.venue,
                activityTitle: activity.title,
                title: activityPackages[ticketType].title,
                price: activityPackages[ticketType].price,
                bookingAvailability: `${formatDate(
                  activityPackages[ticketType].bookingAvailableStart
                )} - ${formatDate(
                  activityPackages[ticketType].bookingAvailableEnd
                )}`,
                validDate: `${formatDate(
                  activityPackages[ticketType].validatedDateStart
                )} - ${formatDate(
                  activityPackages[ticketType].validatedDateEnd
                )}`,
                description: activityPackages[ticketType].description,
                quantity: 1, // Start with quantity 1
              };

              selectedTickets.push(selectedTicket);
            }

            // Update selectedTickets array in local storage
            localStorage.setItem(
              "selectedTickets",
              JSON.stringify(selectedTickets)
            );
          }
        });
      });
      //Add Button............................................................................................................................

      //Minus Button............................................................................................................................

      minusButton.forEach((button, index) => {
        const ticketType = index; // Using index as the ticket type identifier

        button.addEventListener("click", function () {
          if (ticketCounts[ticketType] > 0) {
            ticketCounts[ticketType]--;
            totalTicketPrice -= ticketPrices[ticketType];

            updateTotals();
          }

          //For LocalStorage

          const selectedTicketIndex = selectedTickets.findIndex((ticket) => {
            return (
              ticket.activityId === activityId &&
              ticket.activityImage === activity.image &&
              ticket.venue === activity.venue &&
              ticket.activityTitle === activity.title &&
              ticket.title === activityPackages[ticketType].title &&
              ticket.price === activityPackages[ticketType].price &&
              ticket.bookingAvailability ===
                `${formatDate(
                  activityPackages[ticketType].bookingAvailableStart
                )} - ${formatDate(
                  activityPackages[ticketType].bookingAvailableEnd
                )}` &&
              ticket.validDate ===
                `${formatDate(
                  activityPackages[ticketType].validatedDateStart
                )} - ${formatDate(
                  activityPackages[ticketType].validatedDateEnd
                )}` &&
              ticket.description === activityPackages[ticketType].description
            );
          });

          if (selectedTicketIndex !== -1) {
            if (selectedTickets[selectedTicketIndex].quantity > 1) {
              selectedTickets[selectedTicketIndex].quantity--;
            } else {
              selectedTickets.splice(selectedTicketIndex, 1);
            }
          }

          // Update selectedTickets array in local storage
          localStorage.setItem(
            "selectedTickets",
            JSON.stringify(selectedTickets)
          );
        });
      });

      //Minus Button............................................................................................................................

      // Update total Cost
      function updateTotals() {
        const totalTicketCount = Object.values(ticketCounts).reduce(
          (sum, count) => sum + count,
          0
        );
        totalTickets.textContent = `${totalTicketCount} ticket${
          totalTicketCount !== 1 ? "s" : ""
        }`;
        totalPrice.textContent = `NT$ ${totalTicketPrice}`;
      }
    })
    // Update total Cost

    .catch(function (error) {
      console.error("Error fetching activities:", error);
    });
});

///////////////////////////////////////////////////////////////////////////////////Add the functionality for the add/minus buttons

//More info / Hide info Toggling//////////////////////////////////////////////////////////////////

document.addEventListener("DOMContentLoaded", function () {
  const accordionItems = document.querySelectorAll(".accordion-item");

  accordionItems.forEach((item) => {
    const button = item.querySelector(".accordion-button");
    const buttonText = button.textContent;

    item.addEventListener("show.bs.collapse", function () {
      button.textContent = "Hide Info";
    });

    item.addEventListener("hide.bs.collapse", function () {
      button.textContent = buttonText;
    });
  });
});

//Go to pay - Register Button////////////////////////////////////////////////////////////////////////////////////

var GetTicketUrl = `${baseUrl}Ticket/${activityId}/book`;
var jwtToken = localStorage.getItem("token");

function CheckTicketAvailible() {
  var selectedPackages = localStorage.getItem("selectedTickets");
  const selectedPackagesArray = JSON.parse(selectedPackages);

  if (selectedPackagesArray && selectedPackagesArray.length > 0) {
    const ticketRequiredArray = selectedPackagesArray.map((item) => ({
      packageTitle: item.title,
      quantity: item.quantity,
    }));

    console.log(ticketRequiredArray);

    $.post({
      url: GetTicketUrl,
      dataType: "json",
      contentType: "application/json",
      data: JSON.stringify(ticketRequiredArray),
      beforeSend: function (xhr) {
        // Set the Authorization header with the JWT token
        xhr.setRequestHeader("Authorization", "Bearer " + jwtToken);
      },

      success: (res) => {


        if (res.length > 0) {
          localStorage.setItem("ticketInfo", JSON.stringify(res));

          const totalPriceElement = document.querySelector(".total-price");
          const totalPriceText = totalPriceElement.textContent;
          const numericPart = totalPriceText.match(/\d+/);
          const totalPrice = parseInt(numericPart, 10);
          localStorage.setItem("totalPrice", totalPrice);

          //Websocket Update ticket count in real time

          ticketStore.updateTickets(activityId, ticketRequiredArray);

          window.location.href = `https://cofstyle.shop/payment/Payment.html?id=${activityId}`;

        } else {
          toastr["warning"]("票數不足", "請重新確認票數");
        }

      },

      error: (err) => {
        console.log(err);

        if (err.status === 400) {
          // Handle client-side validation errors
          toastr["warning"]("票數不足", "請刷新頁面");
        } else if (err.status === 409) {
          // Handle concurrency conflicts
          toastr["warning"]("伺服器繁忙,請稍後再試", "Warning");
        } else {
          // Handle other server errors
          toastr["error"]("伺服器異常,請稍後再試", "Error");
        }
      },
    });
  }
}

///HUb..............................................................................................................................

class TicketStore {
  constructor() {
    this.hubConnection = null;
  }

  // Method to create the hub connection
  createHubConnection(activityId) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${baseUrl}ticket?activityId=${activityId}`, {
        accessTokenFactory: () => accessToken,
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.hubConnection
      .start()
      .catch((error) => console.log("Error establishing connection:", error));

    this.hubConnection.on("ReturnUpdatedTickets", (updatedTicketCountData) => {
      console.log(
        `Successfully receive from hub method....!!,${updatedTicketCountData}`
      );
      // Update the UI with the updated ticket count
      updateTicketCountNumber(updatedTicketCountData);
    });
  }

  stopHubConnection() {
    this.hubConnection
      .stop()
      .catch((error) => console.log("Error stopping connection: ", error));
  }

  async updateTickets(activityId, ticketCountData) {
    try {
      console.log("hub update init method!!");
      await this.hubConnection.invoke("UpdateTickets", {
        activityId: activityId,
        ticketCheckingDtos: ticketCountData,
      });
    } catch (error) {
      console.log(error);
    }
  }
}

const ticketStore = new TicketStore();
ticketStore.createHubConnection(activityId);

function updateTicketCountNumber(updatedTicketCountData) {
  updatedTicketCountData.forEach((ticketCountData) => {
    console.log(ticketCountData);

    const packageTitle = ticketCountData.packageTitle;
    const count = ticketCountData.count;
    console.log(packageTitle, count);

    // Find the corresponding ticket count element using the package title
    const ticketCountElement = document.getElementById(
      `ticketCount-${packageTitle}`
    );

    if (ticketCountElement) {
      // Update the ticket count element's content
      ticketCountElement.textContent = `剩餘票數 : ${count} 張`;
    }

    if (count == 0) {
      const addButton = document.getElementById(`addButton-${packageTitle}`);
      const minusButton = document.getElementById(
        `minusButton-${packageTitle}`
      );

      if (addButton && minusButton) {
        // Set the display property based on the count value
        addButton.style.display = count === 0 ? "none" : "block";
        minusButton.style.display = count === 0 ? "none" : "block";
      }
    }
  });
}
///HUb..............................................................................................................................
