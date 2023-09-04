let thisUser = "";
// const overlay = document.querySelector(".overlay");


    $(document).ready(function () {
      //check user
      if (accessToken) {
        $.get({
          url: `${baseUrl}Account`,
          dataType: "json",
          contentType: "application/json",
          beforeSend: function (xhr) {
            // Set the Authorization header with the JWT token
            xhr.setRequestHeader("Authorization", "Bearer " + accessToken);
          },
          success: (res) => {
            thisUser = res.userName;
            //Get Activities
            SearchActivities();
          },
          error: (xhr, status, error) => {
            try {
              // Attempt to handle the error gracefully without console output
              // Log your custom message
              console.log("just..no login user right now");

              // Continue processing as needed
              SearchActivities();
            } catch (e) {
              // Handle any other unexpected errors here
              console.error(""); // Log an empty string or a harmless message
            }
          },
        });
      } else {
        SearchActivities();
      }
    });

// Card Creating
function createCard(activity, formattedDate, ifhost) {
  let ticketCounts = 0;
  for (const ticketPackage of activity.ticketPackages) {
    for (const ticket of ticketPackage.tickets) {
      if (ticket.status === "Available") {
        ticketCounts++;
      }
    }
  }
  const deleteButton = ifhost
    ? `<div class="ftr text-center delete-btn"> 
              <a href="#" class="btn btn-black btn-round btn-danger" data-id="${activity.id}">Delete</a> 
          </div>`
    : "";

  return `
              <div class="card mt-5 card-blog">
                  <div class="card-image" style="width: 380px; height: 250px;">
                      
                      <a href="#"> <img class="img activity-image" src="${activity.image}"> </a>
                      <div class="ripple-cont"></div>
                  </div>
                  <div class="table">
                      <h6 class="category text-warning">
                                  <i class="fa fa-soundcloud"></i> ${activity.category}
                              </h6>
                      <h4 class="card-caption">
                      <a href="#">${activity.description}</a>
                  </h4>
                      <div class="ftr">

                      </div>
                          <div class ="statusss">
                              <div class="stats"> <i class="fa-solid fa-calendar"></i> ${formattedDate} </div>
                              <div class="stats"> <i class="fa-sharp fa-solid fa-heart"></i> ${activity.attendees.length} </div>
                              <div class="stats"> <i class="fa-solid fa-ticket"></i> ${ticketCounts} </div>
                          </div>
                          <div class="btn-wrapper">
                              <div class="ftr text-center"> <a href="https://cofstyle.shop/detail/Activity-Detail.html?id=${activity.id}" class="btn btn-black btn-round btn-info">View</a> </div>
                              ${deleteButton}
                          </div>
                  </div>
              </div>
          `;
}

//search activities
function SearchActivities() {
  $.get({
    url: `${baseUrl}Activities`,
    dataType: "json",
    contentType: "application/json",
    success: (response) => {
      const activities = response;
      console.log(activities);
      activities.forEach(function (activity, index) {
        //DateTime Formatted
        const activityDate = new Date(activity.date);
        const formattedDate = `${activityDate.getFullYear()}-${
          activityDate.getMonth() + 1
        }-${activityDate.getDate()}`;

        //Check if Host for each activity(return yes or no to vanish the delete button)
        //need user and activityId
        let isHost = false;
        if (activity.hostUserName == thisUser) {
          isHost = true;
        }

        //Card Creating
        const newCardHTML = createCard(activity, formattedDate, isHost);
        const columnIndex = index % 3;

        $(".col-md-4").eq(columnIndex).append(newCardHTML);
      });
    },
    error: (error) => {
      console.error("Error fetching activities:", error);
    },
  });

  //Delete Button
  $(document).on("click", ".delete-btn", function () {
    const activityId = $(this).find("a").data("id");
    console.log(this);
    const card = $(this).closest(".card-blog");

    // Confirm before deleting
    if (confirm("Are you sure you want to delete this activity?")) {
        // Send DELETE request to API

        console.log(accessToken+"token is valid now!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!")

        $.ajax({
          url: `${baseUrl}Activities/${activityId}`,
          type: "DELETE",
          headers: {
            Authorization: "Bearer " + accessToken,
          },
          success: function (response) {
            // Remove the deleted card from the UI
            card.remove();
          },
          error: function (error) {
            console.error("Error deleting activity:", error);
          },
        });

    }
  });
}

