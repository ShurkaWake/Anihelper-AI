<script>
import {defineComponent} from "vue";
import axios from "axios";
import store from "@/store";

export default defineComponent({
  data: () => ({
    fullName: "",
    username: "",
    email: "",
    password: "",
    repeatPassword: "",
    loading: false,
    show: false
  }),

  methods: {
    async register(){
      this.loading = true
      await axios.post('auth/register', {
        fullName: this.fullName,
        username: this.username,
        email: this.email,
        password: this.password,
        repeatPassword: this.repeatPassword
      })
          .then(response => {
            store.commit('setErrorMessage', "Ви успішно зареєстровані")
            store.commit('setRedirectPath', "/login")
            store.commit('switchDialog')
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })

      this.loading = false
    }
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
      <h1 id="form-title">Register</h1>
      <v-text-field
          v-model="fullName"
          color="deep-purple-darken-1"
          label="Full Name"
          required>
      </v-text-field>
      <v-text-field
          v-model="email"
          color="deep-purple-darken-1"
          label="Email"
          required>
      </v-text-field>
      <v-text-field
          v-model="username"
          color="deep-purple-darken-1"
          label="Username"
          required>
      </v-text-field>
      <v-text-field
          v-model="password"
          color="deep-purple-darken-1"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :type="show ? 'text' : 'password'"
          label="Password"
          @click:append="show = !show">
      </v-text-field>
      <v-text-field
          v-model="repeatPassword"
          color="deep-purple-darken-1"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :type="show ? 'text' : 'password'"
          label="Repeat Password"
          @click:append="show = !show">
      </v-text-field>
      <v-container
          class="d-flex justify-end">
        <v-btn
            :loading="loading"
            @click="register"
            color="deep-purple-darken-1"
            type="submit"
            text="Register">
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