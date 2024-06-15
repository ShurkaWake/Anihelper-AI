import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'
import axios from "axios";
import { loadFonts } from './plugins/webfontloader'

loadFonts()
axios.defaults.baseURL = "https://localhost:7130/api"
axios.defaults.headers.post['Content-Type'] = "application/json"

createApp(App)
  .use(router)
  .use(store)
  .use(vuetify)
  .mount('#app')
