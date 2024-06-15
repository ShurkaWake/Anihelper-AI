<script setup>

defineEmits(['drawerClick'])
</script>

<script>
import {defineComponent} from "vue";
import {mapActions, mapMutations, mapState} from "vuex";

export default defineComponent({
  methods: {
    ...mapMutations({
      changeDrawerState: "changeDrawerState"
    })
  },

  computed: {
    ...mapState({
      isAuth: state => state.auth.isAuth,
    })
  }
})
</script>

<template>
  <v-app-bar
      app
      class="bg-green-darken-1 align-center"
      height="80"
  >
    <v-col cols="auto">
      <v-app-bar-nav-icon
          @click.stop="changeDrawerState"
          :disabled="!isAuth"
      />
    </v-col>
    <v-col cols="auto">
      <router-link
          v-if="isAuth"
          class="text-decoration-none text-white"
          to="/">
        <v-toolbar-title>Anihelper AI</v-toolbar-title>
      </router-link>
      <v-toolbar-title v-if="!isAuth">Anihelper AI</v-toolbar-title>
    </v-col>

  </v-app-bar>
</template>

<style>
</style>