export const drawerModule = {
    state: {
        drawer: false
    },
    mutations: {
        changeDrawerState(state, path){
            state.drawer = !state.drawer
        },
        closeDrawer(state, path){
            state.drawer = false
        },
        openDrawer(state, path){
            state.drawer = true
        },
    }
}