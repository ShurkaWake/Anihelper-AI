<script>
import {defineComponent} from "vue";
import axios from "axios";
import store from "@/store";
import CategoryViewCard from "@/components/category/CategoryViewCard.vue";

export default defineComponent({
  components: {CategoryViewCard},

  data: () => ({
    categoryList: [],
  }),

  methods: {
    async fetchCategories(){
      await axios.get("/category/all")
          .then(response => {
            this.categoryList = response.data.data
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors[0])
            store.commit('setRedirectPath', "/")
            store.commit('switchDialog')
          })
    },
  },
  mounted() {
    this.fetchCategories()
  },
})
</script>

<template>
  <v-container class="d-flex flex-column justify-center" id="main">
    <v-col
        id="col"
        class="justify-center"
        v-for="item in categoryList"
        :key="item.id"
    >
      <category-view-card
        :category="item"
        @categoryDeleted="fetchCategories"
      />
    </v-col>
  </v-container>

</template>

<style scoped>
#main{
  max-width: 600px;
}
</style>