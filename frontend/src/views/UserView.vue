<script>
import {defineComponent} from "vue";
import {mapState} from "vuex";
import ChangePasswordCard from "@/components/user/ChangePasswordCard.vue";

export default defineComponent({
  components: {ChangePasswordCard},

  computed: {
    ...mapState({
      fullName: state => state.auth.userFullName,
      email: state => state.auth.userEmail,
      role: state => state.auth.userRole
    })
  },
})
</script>

<template>
  <v-container class="d-flex flex-row justify-center">
    <v-col id="col">
      <v-card
          prepend-icon="mdi-account"
          class="pa-2 ma-4"
          v-bind:title='fullName'>
        <v-card-subtitle>{{email}}</v-card-subtitle>
        <v-card-text>{{this.role}}</v-card-text>
      </v-card>
      <v-spacer/>
      <change-password-card class="ma-4"/>
    </v-col>
  </v-container>
</template>

<style scoped>
#col{
  max-width: 600px;
}
</style>