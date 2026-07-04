
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "route": "/"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 4696, hash: '2faf47f363df25ea5e53afe37ef5fe17490c6d9b2c299039e6196bfdc92cfa19', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1004, hash: '02e72396172b68b75caa8e7c1993a1275cefaf05b108d01d5c92fae1564f0d8a', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'index.html': {size: 9566, hash: '0c19f2a3d041b4b2201b8ab1f3357d3923c68f1197e496feaa771885f439acb3', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'styles-SSKQ7WUL.css': {size: 13317, hash: 'CqhWrchtzEU', text: () => import('./assets-chunks/styles-SSKQ7WUL_css.mjs').then(m => m.default)}
  },
};
