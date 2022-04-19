<?php
  include 'db-conn.php';

  $get_query = "SELECT * FROM ffbulletin ORDER BY id DESC LIMIT 10";
  $result = mysqli_query($mysqli, $get_query) or die("Get failed");

  if ($result->num_rows > 0) {
    while($row = $result->fetch_assoc()) {
      echo $row["id"] . "," . $row["comment"] . "\r\n";
    }
  } else {
    echo "0 results";
  }
  $mysqli->close();
?>