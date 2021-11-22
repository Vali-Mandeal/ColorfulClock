<template >
  <div
    class="wrapper"
    v-bind:style="{
      color: color.foreground,
      'background-color': color.background,
    }"
  >
    <div id="timeText">{{ time }}</div>
  </div>
</template>

<script>
import * as signalR from "@aspnet/signalr";

export default {
  name: "App",
  data: () => ({

    time: "No time available",
    color: {
      background: "#ccc",
      foreground: "red",
    },
  }),

  created() {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5243/coloredClockHub")
      .configureLogging(signalR.LogLevel.Information)
      .build();
    connection.start().catch(function (err) {
      return console.error(err.toString());
    });

    connection.on("ReceiveTime", (message) => {
      console.log("time")
      console.log(message)
      this.$data.time = message;
    });

    connection.on("ReceiveColor", (message) => {
      console.log("color")
      console.log(message)
      this.$data.color = message.result;
    });
  },
};
</script>

<style>
* {
  margin: 0;
  padding: 0;

  font-family: "Roboto", sans-serif;
}

.wrapper {
  height: 100vh;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

#timeText {
  font-size: 100px;
}
</style>
