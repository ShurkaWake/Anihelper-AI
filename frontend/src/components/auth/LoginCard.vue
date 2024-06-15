<script>
import {mapActions, mapMutations, mapState} from "vuex";
import {defineComponent} from "vue";
export default defineComponent({
  data: () => ({
    show: false,
    email: "",
    password: "",
  }),

  computed: {
    ...mapState({
      loading:"loading"
    }),
  },

  methods: {
    ...mapMutations({
      setEmail: "setLoginEmail",
      setPassword: "setLoginPassword",
    }),
    ...mapActions({
      login: "login",
      logout: "logout"
    }),
    submitLogin(){
      this.login()
    }
  },

  mounted() {
    this.logout()
  }
})
</script>

<template>
  <v-card class="pa-4" width="400">
    <v-form
        fast-fail
        id="login-form"
        @submit.prevent
    >
      <h1 id="form-title">Log In</h1>
      <v-text-field
          v-model="email"
          @update:model-value="setEmail"
          color="deep-purple-darken-1"
          label="Email"
          required>
      </v-text-field>
      <v-text-field
          v-model="password"
          @update:model-value="setPassword"
          color="deep-purple-darken-1"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :type="show ? 'text' : 'password'"
          label="Password"
          @click:append="show = !show">
      </v-text-field>
      <v-container
          class="d-flex justify-end">
        <router-link to="/register">
          <v-btn
              color="deep-purple-darken-1"
              variant="text"
              text="Register">
          </v-btn>
        </router-link>
        <v-btn
            :loading="loading"
            @click="submitLogin"
            color="deep-purple-darken-1"
            type="submit"
            text="Log In">
        </v-btn>
      </v-container>
    </v-form>
  </v-card>
</template>

<style scoped>
#form-title {
  width: inherit;
  padding: 8px;
  text-align: center;
}
</style>