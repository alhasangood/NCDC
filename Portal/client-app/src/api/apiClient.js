import axios from "axios";

const instance = axios.create();

instance.defaults.baseURL = "/api";

export default instance;
