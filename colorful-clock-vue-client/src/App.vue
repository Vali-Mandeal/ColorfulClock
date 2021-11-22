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
import { ref } from "@vue/reactivity";
import { onMounted } from "@vue/runtime-core";

export default {
  setup() {
    const time = ref("No time available");
    const color = ref({ background: "#320E3B", foreground: "#DBFCFF" });
    let connection;
    const receiveTime = "ReceiveTime"
    const receiveColor = "ReceiveColor"

    onMounted(() => {
      setHubConnection();
      startHubConnection();
      subscribeToHubEvents(receiveTime);
      subscribeToHubEvents(receiveColor);
    });

    const setHubConnection = () => {
      connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5243/coloredClockHub")
        .configureLogging(signalR.LogLevel.Information)
        .build();
    };

    const startHubConnection = () => {
      connection.start().catch(function (err) {
        return console.error(err.toString());
      });
    };

    const subscribeToHubEvents = (eventName) => {
      connection.on(eventName, (message) => {
        if (eventName == receiveTime) time.value = message;
        else if (eventName == receiveColor) color.value = message.result;
      });
    };

    return { time, color };
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
  font-size: 120px;
}
</style>
