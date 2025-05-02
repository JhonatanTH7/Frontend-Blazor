// wwwroot/js/splash.js
window.splashControl = {
    hasSeenSplash: function () {
      return sessionStorage.getItem("splashShown") === "true";
    },
    setSplashShown: function () {
      sessionStorage.setItem("splashShown", "true");
    }
  };
  