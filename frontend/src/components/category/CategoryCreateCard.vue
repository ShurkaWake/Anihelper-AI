<script>
import {defineComponent} from "vue";
import axios from "axios";
import store from "@/store";

export default defineComponent({
  data: () => ({
    name: "",
    description: "",
    loading: false,
  }),

  methods: {
    async createCategory(){
      this.loading = true
      await axios.post('category', {
        name: this.name,
        description: this.description
      })
          .then(response => {
            store.commit('setErrorMessage', "Category has been successfully created")
            store.commit('setRedirectPath', "/category")
            store.commit('switchDialog')
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })

      this.loading = false
    },
  },
})
</script>

<template>
  <v-card class="pa-4" width="400">
    <v-form
        fast-fail
        @submit.prevent
    >
      <h1 id="form-title">Create Category</h1>
      <v-text-field
          v-model="name"
          color="deep-purple-darken-1"
          label="Name"
          required>
      </v-text-field>
      <v-text-field
          v-model="description"
          color="deep-purple-darken-1"
          label="Description"
          required>
      </v-text-field>
      <v-container
          class="d-flex justify-end">
        <v-btn
            :loading="loading"
            @click="createCategory"
            color="deep-purple-darken-1"
            type="submit"
            text="Create">
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