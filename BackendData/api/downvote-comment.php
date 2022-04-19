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

  $id = $_POST["id"];

  $update_query = "UPDATE ffbulletin SET downvotes = downvotes + 1 WHERE id = " . $id . "";
  $result = mysqli_query($mysqli, $update_query) or die("Update failed");

  echo 'success';

  $mysqli->close();
?>