<?php
  $servername = "139.162.162.205:30114";
  $username = "neander";
  $password = "banaan123!";
  $dbname = "db";

  $mysqli = new mysqli($servername, $username, $password, $dbname);

  if ($mysqli -> connect_errno) {
    echo "Failed to connect to MySQL: " . $mysqli -> connect_error;
    exit();
  }

  $get_query = "SELECT * FROM ffbulletin LIMIT 10";
  $result = mysqli_query($mysqli, $get_query) or die("Get failed");

  if ($result->num_rows > 0) {
    while($row = $result->fetch_assoc()) {
      echo $row["id"] . "," . $row["comment"] . "," . "\r\n";
    }
  } else {
    echo "0 results";
  }
  $mysqli->close();
?>