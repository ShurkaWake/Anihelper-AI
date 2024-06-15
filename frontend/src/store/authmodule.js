import axios from "axios";
import store from "@/store/index";
import router from "@/router";

export const authModule = {
    state: () => ({
        userId: "",
        userFullName: "",
        userRole: "",
        userEmail: "",
        userName: "",
        isAuth: false,
        loading: false,

        loginEmail: "",
        loginPassword: "",

    }),
    mutations: {
        setUserData(state, user){
            state.userId = user.id
            state.userFullName = user.fullName
            state.userRole = user.role
            state.userEmail = user.email
            state.username = user.username
            state.isAuth = true
        },

        setFullName(state, fullname){
            state.userFullName = fullname
        },

        setAuth(state, status){
            state.isAuth = status
        },

        setLoading(state, status) {
            state.loading = status
        },

        setLoginEmail(state, email){
            state.loginEmail = email
        },

        setLoginPassword(state, password)
        {
            state.loginPassword = password
        }
    },
    actions: {
        async login({state, commit}){
            commit('setLoading', true)
            await axios.post('auth/login',
                {
                    email: state.loginEmail,
                    password: state.loginPassword
                })
                .then(response => {
                    store.commit('setUserData', response.data)
                    axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`
                    localStorage.setItem("Auth", response.data.token)
                    router.push("/")
                })
                .catch(response => {
                    store.commit('setErrorMessage', response.response.data.errors[0])
                    store.commit('setRedirectPath', "")
                    store.commit('switchDialog')
                })

            commit('setLoading', false)
        },

        async logout({state, commit}) {
            commit('setLoading', true)
            commit('setUserData', {
                id: "",
                fullName: "",
                role: "",
                email: "",
            })
            commit('setAuth', false)
            axios.defaults.headers.common['Authorization'] = null
            localStorage.removeItem("Auth")
            commit('setLoading', false)
        },

        async fetchStorage({state, commit}){
            commit('setLoading', true)
            let token = localStorage.getItem("Auth")

            if (token === null){
                return
            }
            axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
            await axios.get("user")
                .then(response => {
                    store.commit('setUserData', response.data.data)
                })
                .catch(response => {
                    console.log('ahaha')
                    commit('setUserData', {
                        id: "",
                        fullName: "",
                        role: "",
                        email: "",
                        token: "",
                    })
                    commit('setAuth', false)
                    axios.defaults.headers.common['Authorization'] = null
                    localStorage.removeItem("Auth")
                })
            commit('setLoading', false)
        }
    }
}