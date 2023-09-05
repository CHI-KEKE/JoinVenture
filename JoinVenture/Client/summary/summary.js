
if (!accessToken) {
  openPopup();
}
else{

  //Get USersList

  $.ajax({
    url: `${baseUrl}Account/userlist`,
    type: "GET",
    dataType: "json",
    contentType: "application/json",
    beforeSend: function (xhr) {
      xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
    },
    success: (users) => {
      console.log(users);
      users.forEach((user, index) => {
        const accordionItem = createUserSummary(user, index);

        const tableBody = accordionItem.querySelector(".OrderInfo");

        user.orders.forEach((order) => {
          const row = document.createElement("tr");
          row.innerHTML = `
                <td class="orderId">${order.id}</td>
                <td class="orderInvoiceDate" style="padding-top: 20px;">${formatDate(
                  order.invoiceDate
                )}</td>
                <td class="orderActivityTitle" style="padding-top: 20px;">${
                  order.activityTitle
                }</td>
            `;
          tableBody.appendChild(row);
        });

        accordionContainer.appendChild(accordionItem);
      });
    },
    error: (xhr, status, error) => {
      if (xhr.status === 403) {
        toastr["warning"]("你不是管理員!", "Warning");
      } else {
        console.log("other error");
      }
    },
  });
}



function formatDate(dateString) {
  const date = new Date(dateString);
  const year = date.getFullYear();
  const month = (date.getMonth() + 1).toString().padStart(2, "0");
  const day = date.getDate().toString().padStart(2, "0");
  const hours = date.getHours().toString().padStart(2, "0");
  const minutes = date.getMinutes().toString().padStart(2, "0");
  return `${year}-${month}-${day} ${hours}:${minutes}`;
}

// Get the accordion container element
const accordionContainer = document.querySelector(".accordion-container");

// Create a template string for the accordion item
function createUserSummary(user, index) {
    const accordionItem = document.createElement("div");
    accordionItem.classList.add("accordion-item");
    accordionItem.innerHTML = `
        <div class="accordion-header">
            <div class="accordion-title">
                <h5 class="p-3 userOrderSummary">${user.showName}</h5>
            </div>
        </div>
        <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapse${index}" aria-expanded="true" aria-controls="collapse${index}" style="color:#5D99E2;">
            Order Detail
        </button>
        <div id="collapse${index}" class="accordion-collapse collapse" data-bs-parent="#accordion-container">
            <!-- Accordin for PackageDetail -->
            <div class="accordion-body">
                <table class="table table-hover text-nowrap">
                    <thead>
                        <tr>
                            <th>OrderId</th>
                            <th>InvoiceTimeStamp</th>
                            <th>ActivityTitle</th>
                        </tr>
                    </thead>
                    <tbody class="OrderInfo">
                        <!-- Each Package row -->
                        
                        <!-- Each Package row -->
                        
                    </tbody>
                </table>
            </div>
        </div>
    `;
    return accordionItem;
}


