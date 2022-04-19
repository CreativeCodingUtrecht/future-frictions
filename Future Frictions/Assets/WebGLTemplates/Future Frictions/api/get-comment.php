<?php
  include 'db-conn.php';

  $comment = $_POST["comment"];

  $get_query = "SELECT id FROM `ffbulletin` WHERE comment LIKE '" . $comment . "';";
  $result = mysqli_query($mysqli, $get_query) or die("Get failed");

  if ($result->num_rows > 0) {
    $row = $result->fetch_assoc();
    echo $row["id"];
  } else {
    echo "no data";
  }

  $mysqli->close();
?>
