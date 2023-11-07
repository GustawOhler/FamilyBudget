const path = require("path");
/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  sassOptions: {
    includePaths: ["node_modules/bootstrap/scss/", path.join(__dirname, "styles")],
  },
};

module.exports = nextConfig;
