<script>
import {defineComponent} from "vue";
import axios from "axios";
import store from "@/store";

export default defineComponent({
  data: () => ({
    category: {
      id: 0,
      name: "",
      description: ""
    },
    name: "",
    description: "",
    loading: false,
  }),

  methods: {
    async editCategory(){
      this.loading = true
      await axios.put('category/' + this.$route.params.id, {
        name: this.name,
        description: this.description
      })
          .then(response => {
            store.commit('setErrorMessage', "Category has been successfully edited")
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

    async fetchCategory(){
      await axios.get('category/' + this.$route.params.id)
          .then(response => {
            this.category = response.data.data
            this.name = this.category.name
            this.description = this.category.description
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "/category")
            store.commit('switchDialog')
          })
    }
  },

  async mounted(){
    await this.fetchCategory()
  }
})
</script>

<template>
  <v-card class="pa-4" width="400">
    <v-form
        fast-fail
        @submit.prevent
    >
      <h1 id="form-title">Edit Category</h1>
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
            @click="editCategory"
            color="deep-purple-darken-1"
            type="submit"
            text="Edit">
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