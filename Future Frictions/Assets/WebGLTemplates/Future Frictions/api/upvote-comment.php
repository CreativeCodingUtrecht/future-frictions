<?php
  include 'db-conn.php';

  $id = $_POST["id"];

  $update_query = "UPDATE ffbulletin SET upvotes = upvotes + 1 WHERE id = " . $id . "";
  $result = mysqli_query($mysqli, $update_query) or die("Update failed");

  echo 'success';

  $mysqli->close();
?>
