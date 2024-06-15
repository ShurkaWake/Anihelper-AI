<script>
import {defineComponent} from "vue";
import {mapActions, mapMutations, mapState} from "vuex";
import NavigationButton from "@/components/main-layout/NavigationButton.vue";

export default defineComponent({
  methods: {
    ...mapActions({
      logoutAction: "logout"
    }),

    ...mapMutations({
      closeDrawer: "closeDrawer"
    }),

    exit() {
      this.closeDrawer()
      this.logoutAction()
    }
  },
  components: {NavigationButton},

  computed: {
    ...mapState({
      isAuth: state => state.auth.isAuth,
      fullName: state => state.auth.userFullName,
      email: state => state.auth.userEmail,
      role: state => state.auth.userRole,
      drawer: state => state.drawer.drawer,
    }),

    isAdmin(){
      return this.role === "admin"
    },
  },
})
</script>

<template>
  <v-navigation-drawer
      width="400"
      v-model="drawer"
      location="start"
  >
    <v-container>
      <v-container v-if="isAuth"
                   class="d-flex flex-column">
        <router-link
            to="/profile"
            class="text-decoration-none"
        >
          <v-card
              prepend-icon="mdi-account"
              class="pa-2"
              v-bind:title='fullName'
          >
            <v-card-subtitle>{{email}}</v-card-subtitle>
          </v-card>
        </router-link>
        <NavigationButton
            link="/video/create"
            text="Create video"
            icon="mdi-video-plus"
        />
        <NavigationButton
            link="/video"
            text="My videos"
            icon="mdi-playlist-play"
        />
        <NavigationButton
            link="/category/create"
            v-if="isAdmin"
            text="Create Category"
            icon="mdi-tag-plus"
        />
        <NavigationButton
            link="/category"
            v-if="isAdmin"
            text="View Categories"
            icon="mdi-tag-multiple"
        />
        <v-divider/>
        <NavigationButton
            link="/login"
            text="Log Out"
            icon="mdi-logout"
        />
      </v-container>
    </v-container>
  </v-navigation-drawer>
</template>

<style scoped>

</style>