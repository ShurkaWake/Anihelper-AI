import { createStore } from 'vuex'
import {authModule} from "@/store/authmodule";
import {dialogModule} from "@/store/dialogmodule";
import {drawerModule} from "@/store/drawermodule";

export default createStore({
  state: {
  },
  getters: {
  },
  mutations: {
  },
  actions: {
  },
  modules: {
    auth: authModule,
    dialog: dialogModule,
    drawer: drawerModule,
  }
})
