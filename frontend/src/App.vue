<template>
  <v-app>
    <v-layout height="100%" width="100%" class="d-flex flex-column">
      <app-bar @drawer-click="changeDrawerState"/>
      <drawer/>
      <v-main>
        <router-view @closeDrawer="closeDrawer"/>
      </v-main>
    </v-layout>
    <v-dialog
        v-model="dialogState"
        width="auto">
      <v-card>
        <v-card-text>
          {{ message }}
        </v-card-text>
        <v-card-actions>
          <v-btn color="deep-purple-darken-1" block @click="closeDialog">Close</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-app>
</template>

<script>

import AppBar from "@/components/main-layout/AppBar.vue";
import router from "@/router";
import {mapActions, mapMutations, mapState} from "vuex";
import Drawer from "@/components/main-layout/Drawer.vue";

export default {
  name: 'App',
  components: {Drawer, AppBar},

  data: () => ({

  }),

  methods: {
    closeDialog(){
      this.changeDialog()
      if (this.redirectPath !== ""){
        router.push(this.redirectPath)
      }
    },

    ...mapMutations({
      changeDialog:"switchDialog",
      changeDrawerState:"changeDrawerState",
      closeDrawer: "closeDrawer"
    }),

    ...mapActions({
      fetchStorage: "fetchStorage",
      logout:"logout",
    })
  },

  computed: {
    ...mapState({
      dialogState: state => state.dialog.dialog,
      message: state => state.dialog.message,
      redirectPath: state => state.dialog.redirectPath,
      isAuth: state => state.auth.isAuth
    }),
  },

  async mounted() {
    await this.fetchStorage()
    if (!this.isAuth){
      router.push('/login')
      this.logout()
    }
  },
}
</script>
