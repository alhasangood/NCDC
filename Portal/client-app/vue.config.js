module.exports = {
  chainWebpack(config) {
    config.plugin("html").tap((args) => {
      args[0].title = "نظام الشهادة الصحية";
      return args;
    });
  },

  outputDir: "../wwwroot/",
  filenameHashing: false,

  devServer: {
    port: 9090,
    https: false,
    proxy: {
      "^/api": {
            target: 'https://localhost:44301/'
      }
    }
  },

  transpileDependencies: [
    'vuetify'
  ]
}
