//To Check if user is Authenticated
$(document).ready(function () {
  const getUserUrl = `${baseUrl}Account`;

  if (accessToken) {
    $.ajax({
      url: getUserUrl,
      type: "GET",
      dataType: "json",
      contentType: "application/json",
      beforeSend: function (xhr) {
        xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
      },
      success: (res) => {
        //for notification use
        localStorage.setItem("showName", res.showName);
        //Nav part
        const userImage = document.getElementById("userImage");
        if (res.photos[0] != undefined) {
          userImage.innerHTML = `<img src="${res.photos[0].url}" alt="User Image" width="30" height="30" class = "rounded-circle" style="object-fit:cover;object-position: center;">`;
          userImage.style.display = "block";
        }

        const logoutButton = document.getElementById("logoutButton");
        logoutButton.style.display = "block";
        const loginButton = document.getElementById("loginButton");
        loginButton.style.display = "none";

        if (res.userName === "admin") {
          const adminNavItem = document.querySelector(".admin");
          if (adminNavItem) {
            adminNavItem.style.display = "block";
          }
        }

        //////////////////////Profile part
        let phonenumberElement = document.querySelector(".phoneNumber");
        if (phonenumberElement != null) {
          $(".showName").text(res.showName);
          $(".userName").text(res.userName);
          $(".phoneNumber").text(res.phoneNumber);
          $(".mobile").text(res.mobile);
          $(".address").text(res.address);
          $(".userImage").attr("src", res.photos[0].url);

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
            OrderSection.append(orderCard);
          });
        }
        //////////////////////Profile part

        /////////////////////OrderStep3 Part
        let orderStep3UserElement = document.querySelector(".orderUser");
        if (orderStep3UserElement != null) {
          orderStep3UserElement.textContent = res.showName;
        }      
        /////////////////////OrderStep3 Part
      },
      error: (xhr, status, error) => {
        if (err.status === 401)
        {
          console.error("just..not login user now");
        } 
      },
    });
  }
});

//Login Mechanism
function loginNative() {
  wrapper.classList.add("active-popup");
}

function logout() {
  localStorage.removeItem("token");
  toastr["info"]("", "已登出");
  setTimeout(function () {
    window.location.reload();
  }, 1000);
}

