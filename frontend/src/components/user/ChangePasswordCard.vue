<script>
import {defineComponent} from "vue";
import store from "@/store";
import {mapActions} from "vuex";
import axios from "axios";

export default defineComponent({

  data: () => ({
    valid: false,
    show: false,
    loading: false,

    oldPassword: "",
    newPassword: "",
    newPasswordRepeat: "",
  }),

  computed: {
    passwordRules(){
      return [
        value => {
          if (value) return true
          return "Password is required"
        },
        value => {
          if (/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$/.test(value)) return true
          return "Password should be between 6 and 50 characters and contain at least one lowercase letter, " +
              "uppercase letter, special character and numeric symbol"
        },
      ]
    }
  },

  methods: {
    ...mapActions({
      logout:"logout"
    }),

    async changePassword(){
      if (!this.valid){
        return
      }

      this.loading = true
      await axios.patch("/user/password", {
        oldPassword: this.oldPassword,
        newPassword: this.newPassword
      })
          .then(response => {
            store.commit('setErrorMessage', "Password successfully changed")
            store.commit('setRedirectPath', "/login")
            this.logout()
            store.commit('switchDialog')
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors[0])
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })
      this.loading = false
    },

    matchingPasswords() {
      if (this.newPassword === undefined || this.newPasswordRepeat === undefined) {
        return "Password should be same"
      }
      return this.newPassword === this.newPasswordRepeat ? true : "Password should be same";
    },
  }
})
</script>

<template>
  <v-card class="pa-4">
    <v-form
        fast-fail
        v-model="valid"
        validate-on="input"
        @submit.prevent
    >
      <h3 id="form-title">Change Password</h3>
      <v-text-field
          v-model="oldPassword"
          color="yellow-accent-3"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :rules="passwordRules"
          :type="show ? 'text' : 'password'"
          label="Old Password"
          @click:append="show = !show">
      </v-text-field>
      <v-text-field
          v-model="newPassword"
          color="deep-purple-darken-1"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :rules="passwordRules.concat(matchingPasswords)"
          :type="show ? 'text' : 'password'"
          label="New password"
          @click:append="show = !show">
      </v-text-field>
      <v-text-field
          v-model="newPasswordRepeat"
          color="deep-purple-darken-1"
          :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
          :rules="passwordRules.concat(matchingPasswords)"
          :type="show ? 'text' : 'password'"
          label="Repeat new password"
          @click:append="show = !show">
      </v-text-field>
      <v-container
          class="d-flex justify-end">
        <v-btn
            :loading="loading"
            @click="changePassword"
            color="deep-purple-darken-1"
            type="submit"
            text="Confirm">
        </v-btn>
      </v-container>
    </v-form>
  </v-card>
</template>

<style scoped>

</style>