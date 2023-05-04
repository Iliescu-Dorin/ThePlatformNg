   import gulp from 'gulp'
import gulpBrotli from 'gulp-brotli'
import zlib from 'zlib'

export function compressBrotli() {
  return gulp
    .src(`./dist/client-app/**/*.{js,css,html}`)
    .pipe(gulpBrotli({
      extension: 'brotli',
      skipLarger: true,
      // the options are documented at https://nodejs.org/docs/latest-v10.x/api/zlib.html#zlib_class_brotlioptions
      params: {
        // brotli parameters are documented at https://nodejs.org/docs/latest-v10.x/api/zlib.html#zlib_brotli_constants
        [zlib.constants.BROTLI_PARAM_QUALITY]: zlib.constants.BROTLI_MAX_QUALITY,
      },
    }))
    .pipe(gulp.dest(`./dist/compress/client-app/`))
}
