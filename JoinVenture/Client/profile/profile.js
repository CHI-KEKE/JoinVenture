var GetUserUrl = `${baseUrl}Account`;
const overlay = document.querySelector(".overlay");
const wrapper2 = document.querySelector(".wrapper");


const openPopup = function () {
  wrapper2.classList.add("active-popup");
  overlay.style.display = "block";
};

const closePopup = function () {
  wrapper2.classList.remove("active-popup");
  overlay.style.display = "none";
};



$(document).ready(function () {
  $.get({
    url: GetUserUrl,
    dataType: "json",
    contentType: "application/json",
    beforeSend: function (xhr) {
      // Set the Authorization header with the JWT token
      xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
    },
    success: (res) => {
        closePopup();
      // UserProfile Part
      $(".showName").text(res.showName);
      $(".userName").text(res.userName);
      $(".phoneNumber").text(res.phoneNumber);
      $(".mobile").text(res.mobile);
      $(".address").text(res.address);
      $(".userImage").attr("src", res.photos[0].url);

      // User Orders Part
      const OrderSection = $(".OrderSection");
      res.orders.forEach((order) => {
        const orderCard = `
                            <div class="card mb-3">
                                <div class="card-body">
                                    <!-- Activity Title -->
                                    <div class="row">
                                        <div class="col-sm-3">
                                            <h6 class="mb-0 activityName">${
                                              order.activityTitle
                                            }</h6>
                                        </div>
                                    </div>
                                    <hr>

                                    <!-- TicketPackage -->
                                    ${order.bookedTicketPackages
                                      .map(
                                        (package) => `
                                        <div class="ticket m-3">
                                            <div class="left">
                                                <div class="image activityImage" style="background-image: url(${
                                                  order.activityImage
                                                });background-size: cover; background-position: center;">
                                                </div>
                                                <div class="ticket-info">
                                                    <p class="date">
                                                        <span class="validDate">${
                                                          package.validDate
                                                        }</span>
                                                    </p>
                                                    <div class=".ticket-infot">
                                                        <h3 class="ticketTitle">${
                                                          package.title
                                                        }</h3>
                                                        <h5 class="ticketQuantity">${
                                                          package.quantity
                                                        } 張</h5>
                                                    </div>
                                                    <p class="location"><span>祝您愉快</span><i class="far fa-smile"></i></p>
                                                </div>
                                            </div>
                                            <div class="right">
                                                <div class="right-info-container">
                                                    <div class="barcode">
                                                        <img src="https://external-preview.redd.it/cg8k976AV52mDvDb5jDVJABPrSZ3tpi1aXhPjgcDTbw.png?auto=webp&s=1c205ba303c1fa0370b813ea83b9e1bddb7215eb" alt="QR code">
                                                    </div>
                                                    <p class="ticket-number OrderId">#${order.id.substring(
                                                      0,
                                                      8
                                                    )}</p>
                                                </div>
                                            </div>
                                        </div>
                                    `
                                      )
                                      .join("")}
                                    <!-- TicketPackage -->
                                </div>
                            </div>
                        `;

        // Insert the order card after the UserProfile card
        OrderSection.append(orderCard);
      });
    },
    error: (err) => {
        if(err.status === 401)
        {
            openPopup();
        }

        console.log(err);

    },

  });


});




