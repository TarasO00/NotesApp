import { createSlice } from "@reduxjs/toolkit";

const initialState = []

export const notesSlice = createSlice({
    name: "notes",
    initialState,
    reducers: {
        addAll: (state, action) => {
            state.push(...action.payload)
        },
        add: (state, action) => {
            state.push(action.payload)
        },
        update: (state, action) => {
            let i = state.indexOf(x => x.id === action.payload.id)
            state[i] = action.payload
        },
        remove: (state, action) => {
            state.remove(x => x.id === action.payload)
        },
    },
})

export const { addAll, add, update, remove } = notesSlice.actions

export default notesSlice.reducer