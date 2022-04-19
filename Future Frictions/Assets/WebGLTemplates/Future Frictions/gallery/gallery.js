function main() {
  // Get data from backend
  fetch("../api/get-postcards.php")
    .then((res) => res.json())
    .then((data) => createElements(data))
    .catch((err) => console.log(err));
}

function createElements(data) {
  var container = document.getElementById("container");

  if (data.length > 0) {
    for (let i = 0; i < data.length; i++) {
      const element = data[i];

      var newDiv = document.createElement(`div`);
      newDiv.innerHTML = `
        <figure class="imghvr-flip-horiz">
          <img src="${document.location.origin}/api/data/uploads/${element.SessionID}.png" alt="postcard-image" />
          <figcaption>
            ${element.PostcardMessage.replace(/(?:\r\n|\r|\n)/g, "<br>")}
          </figcaption>
        </figure>
      `;
      container.appendChild(newDiv);
    }
  } else {
    var h1 = document.createElement("h1");
    h1.innerText = "No Postcards yet";
    container.appendChild(h1);
  }
}

function stringToHTML(str) {
  var parser = new DOMParser();
  var doc = parser.parseFromString(str, "text/html");
  return doc.body.innerHTML;
}

main();
