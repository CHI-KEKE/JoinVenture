// Card Creating
function createCard(activity, formattedDate) {
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
                            <div class="stats"> <i class="fa-sharp fa-solid fa-heart"></i> ${activity.followers} </div>
                            <div class="stats"> <i class="fa-solid fa-ticket"></i> ${activity.tickets} </div>
                        </div>
                        <div class="btn-wrapper">
                            <div class="ftr text-center"> <a href="https://fff5-2402-7500-4d5-a113-e930-21d7-3d9c-cf18.ngrok-free.app/Client/detail/Activity-Detail.html?id=${activity.id}" class="btn btn-black btn-round btn-info">View</a> </div>
                            <div class="ftr text-center delete-btn"> 
                                <a href="#" class="btn btn-black btn-round btn-danger" data-id="${activity.id}">Delete</a> 
                            </div>
                        </div>
                </div>
            </div>
        `;
}

$(document).ready(function () {
  axios
    .get(`${baseUrl}Activities`)
    .then(function (response) {
      const activities = response.data;

      activities.forEach(function (activity, index) {
        //DateTime Formatted
        const activityDate = new Date(activity.date);
        const formattedDate = `${activityDate.getFullYear()}-${
          activityDate.getMonth() + 1
        }-${activityDate.getDate()}`;

        //Card Creating
        const newCardHTML = createCard(activity, formattedDate);
        const columnIndex = index % 3;

        $(".col-md-4").eq(columnIndex).append(newCardHTML);
      });
    })
    .catch(function (error) {
      console.error("Error fetching activities:", error);
    });

  //Delete Button
  $(document).on("click", ".delete-btn", function () {
    const activityId = $(this).find("a").data("id");
    console.log(this);
    const card = $(this).closest(".card-blog"); // Store reference to the card

    // Confirm before deleting
    if (confirm("Are you sure you want to delete this activity?")) {
      // Send DELETE request to API
      axios
        .delete(`${baseUrl}Activities/${activityId}`)
        .then(function (response) {
          // Remove the deleted card from the UI
          card.remove();
        })
        .catch(function (error) {
          console.error("Error deleting activity:", error);
        });
    }
  });
});

// // Function to update the UI with received comments

function updateUIWithCommentsNotification(comments) {
  const commentSection = document.querySelector(".panel-body");
  commentSection.innerHTML = "";

  // Iterate through comments and update the UI with each comment
  comments.forEach((comment) => {
    const newComment = document.createElement("div");
    newComment.className = "media-block";
    newComment.innerHTML = `
          <a class="media-left" href="#"><img class="img-circle img-sm" alt="Profile Picture" src="${comment.image}"></a>
          <div class="media-body">
              <div class="mar-btm">
                  <a href="#" class="btn-link text-semibold media-heading box-inline">${comment.showName}</a>
                  <p class="text-muted text-sm"><i class="fa fa-mobile fa-lg"></i> - From Mobile - Just now</p>
              </div>
              <p>${comment.body}</p>
              <div class="pad-ver">
                  <div class="btn-group">
                      <a class="btn btn-sm btn-default btn-hover-success" href="#"><i class="fa fa-thumbs-up"></i></a>
                      <a class="btn btn-sm btn-default btn-hover-danger" href="#"><i class="fa fa-thumbs-down"></i></a>
                  </div>
                  <a class="btn btn-sm btn-default btn-hover-primary" href="#">Comment</a>
              </div>
              <hr>
          </div>
      `;
    console.log(newComment);

    commentSection.insertAdjacentElement("afterbegin", newComment);
  });
}

//

// class CommentStore {
//   constructor() {
//     this.comments = [];
//     this.hubConnection = null;
//   }

//   // Method to create the hub connection
//   createHubConnection(activityId) {
//     this.hubConnection = new signalR.HubConnectionBuilder()
//       .withUrl(`http://localhost:5000/chat?activityId=${activityId}`, {
//         accessTokenFactory: () => accessToken,
//       })
//       .withAutomaticReconnect()
//       .configureLogging(signalR.LogLevel.Information)
//       .build();
//     this.hubConnection
//       .start()
//       .catch((error) => console.log("Error establishing connection:", error));
//   }
// }
