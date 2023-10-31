import {configureStore} from "@reduxjs/toolkit";
import notesReducer from "./notesSlice.js";

export const store = configureStore({
    reducer: {
        notes: notesReducer
    },
})