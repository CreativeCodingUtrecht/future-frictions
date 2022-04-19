<?php
  if ($_POST["id"] && $_POST["json"]) {
    $id = $_POST["id"];
    $json = $_POST["json"];

    $save_path="/var/www/html/api/data/json/";
    
    if (!file_exists($save_path)) {
      mkdir($save_path, 0777, true);
    }

    $fileName = $id . ".json";

    if (file_put_contents($save_path . $fileName, $json)) {
      echo "success";
    } else {
      echo "failed";
    }
  } else {
    echo "error";
  }
?>
