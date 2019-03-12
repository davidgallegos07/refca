# Refca
Repositorio Institucional

Intallation Instructions locally
- git clone git@github.com:yourusername/refca.git
- npm install
- dotnet restore
- webpack
- dotnet ef database update
- dotnet run
** make sure your credentials are correct in appsettings.json (database name, user and password)

# Working versions
- webpack --version webpack 3.5.5
- dotnet --info dotnet 2.0 sdk and runtime
- node --version v6.11.0

```
├─┬ @angular/animations@4.4.7
│ └── tslib@1.9.3
├── @angular/common@2.4.10
├── @angular/compiler@2.4.10
├── UNMET PEER DEPENDENCY @angular/core@2.4.10
├── @angular/forms@2.4.10
├── @angular/http@2.4.10
├── @angular/platform-browser@2.4.10
├── @angular/platform-browser-dynamic@2.4.10
├─┬ @angular/platform-server@2.4.10
│ └── parse5@2.2.3
├── @angular/router@3.4.10
├── @types/chai@3.5.2
├── @types/jasmine@2.8.16
├── @types/jquery@2.0.38
├── @types/node@6.14.4
├── @types/underscore@1.8.13
├── angular2-platform-node@2.0.11
├─┬ angular2-template-loader@0.6.2
│ └─┬ loader-utils@0.2.17
│   ├── big.js@3.2.0
│   └── emojis-list@2.1.0
├─┬ angular2-universal@2.1.0-rc.1
│ ├── angular2-platform-node@2.1.0-rc.1
│ ├─┬ js-beautify@1.9.0
│ │ ├─┬ config-chain@1.1.12
│ │ │ ├── ini@1.3.5
│ │ │ └── proto-list@1.2.4
│ │ ├─┬ editorconfig@0.15.3
│ │ │ ├── commander@2.19.0
│ │ │ └── sigmund@1.0.1
│ │ └─┬ nopt@4.0.1
│ │   ├── abbrev@1.1.1
│ │   └─┬ osenv@0.1.5
│ │     └── os-homedir@1.0.2
│ └── xhr2@0.1.4
├─┬ angular2-universal-patch@0.2.1
│ └── semver@5.6.0
├─┬ angular2-universal-polyfills@2.1.0-rc.1
│ ├── es6-promise@3.0.2
│ ├── ie-shim@0.1.0
│ └── reflect-metadata@0.1.2
├─┬ aspnet-prerendering@2.0.6
│ └─┬ domain-task@2.0.3
│   └── domain-context@0.5.1
├─┬ aspnet-webpack@1.0.29
│ ├─┬ connect@3.6.6
│ │ ├─┬ debug@2.6.9
│ │ │ └── ms@2.0.0
│ │ ├─┬ finalhandler@1.1.0
│ │ │ ├── encodeurl@1.0.2
│ │ │ ├── escape-html@1.0.3
│ │ │ ├── statuses@1.3.1
│ │ │ └── unpipe@1.0.0
│ │ ├── parseurl@1.3.2
│ │ └── utils-merge@1.0.1
│ ├── es6-promise@3.3.1
│ ├─┬ memory-fs@0.3.0
│ │ ├─┬ errno@0.1.7
│ │ │ └── prr@1.0.1
│ │ └─┬ readable-stream@2.3.6
│ │   ├── core-util-is@1.0.2
│ │   ├── isarray@1.0.0
│ │   ├── process-nextick-args@2.0.0
│ │   └── util-deprecate@1.0.2
│ ├── require-from-string@1.2.1
│ ├─┬ webpack-dev-middleware@1.12.2
│ │ ├── memory-fs@0.4.1
│ │ ├── path-is-absolute@1.0.1
│ │ └── time-stamp@2.2.0
│ └── webpack-node-externals@1.7.2
├─┬ awesome-typescript-loader@3.5.0
│ ├─┬ chalk@2.4.2
│ │ ├─┬ ansi-styles@3.2.1
│ │ │ └─┬ color-convert@1.9.3
│ │ │   └── color-name@1.1.3
│ │ ├── escape-string-regexp@1.0.5
│ │ └─┬ supports-color@5.5.0
│ │   └── has-flag@3.0.0
│ ├─┬ enhanced-resolve@3.3.0
│ │ └── memory-fs@0.4.1
│ ├─┬ loader-utils@1.2.3
│ │ ├── big.js@5.2.2
│ │ └─┬ json5@1.0.1
│ │   └── minimist@1.2.0
│ ├── lodash@4.17.11
│ ├─┬ micromatch@3.1.10
│ │ ├── arr-diff@4.0.0
│ │ ├── array-unique@0.3.2
│ │ ├─┬ braces@2.3.2
│ │ │ ├── arr-flatten@1.1.0
│ │ │ ├─┬ extend-shallow@2.0.1
│ │ │ │ └── is-extendable@0.1.1
│ │ │ ├─┬ fill-range@4.0.0
│ │ │ │ ├── extend-shallow@2.0.1
│ │ │ │ ├─┬ is-number@3.0.0
│ │ │ │ │ └── kind-of@3.2.2
│ │ │ │ ├── repeat-string@1.6.1
│ │ │ │ └── to-regex-range@2.1.1
│ │ │ ├── isobject@3.0.1
│ │ │ ├── repeat-element@1.1.3
│ │ │ ├─┬ snapdragon-node@2.1.1
│ │ │ │ ├─┬ define-property@1.0.0
│ │ │ │ │ └─┬ is-descriptor@1.0.2
│ │ │ │ │   ├── is-accessor-descriptor@1.0.0
│ │ │ │ │   └── is-data-descriptor@1.0.0
│ │ │ │ └─┬ snapdragon-util@3.0.1
│ │ │ │   └── kind-of@3.2.2
│ │ │ └── split-string@3.1.0
│ │ ├─┬ define-property@2.0.2
│ │ │ └─┬ is-descriptor@1.0.2
│ │ │   ├── is-accessor-descriptor@1.0.0
│ │ │   └── is-data-descriptor@1.0.0
│ │ ├─┬ extend-shallow@3.0.2
│ │ │ ├── assign-symbols@1.0.0
│ │ │ └─┬ is-extendable@1.0.1
│ │ │   └── is-plain-object@2.0.4
│ │ ├─┬ extglob@2.0.4
│ │ │ ├─┬ define-property@1.0.0
│ │ │ │ └─┬ is-descriptor@1.0.2
│ │ │ │   ├── is-accessor-descriptor@1.0.0
│ │ │ │   └── is-data-descriptor@1.0.0
│ │ │ ├─┬ expand-brackets@2.1.4
│ │ │ │ ├── define-property@0.2.5
│ │ │ │ ├── extend-shallow@2.0.1
│ │ │ │ └── posix-character-classes@0.1.1
│ │ │ └── extend-shallow@2.0.1
│ │ ├─┬ fragment-cache@0.2.1
│ │ │ └── map-cache@0.2.2
│ │ ├── kind-of@6.0.2
│ │ ├─┬ nanomatch@1.2.13
│ │ │ └── is-windows@1.0.2
│ │ ├── object.pick@1.3.0
│ │ ├─┬ regex-not@1.0.2
│ │ │ └─┬ safe-regex@1.1.0
│ │ │   └── ret@0.1.15
│ │ ├─┬ snapdragon@0.8.2
│ │ │ ├─┬ base@0.11.2
│ │ │ │ ├─┬ cache-base@1.0.1
│ │ │ │ │ ├─┬ collection-visit@1.0.0
│ │ │ │ │ │ ├── map-visit@1.0.0
│ │ │ │ │ │ └── object-visit@1.0.1
│ │ │ │ │ ├── get-value@2.0.6
│ │ │ │ │ ├─┬ has-value@1.0.0
│ │ │ │ │ │ └─┬ has-values@1.0.0
│ │ │ │ │ │   └── kind-of@4.0.0
│ │ │ │ │ ├─┬ set-value@2.0.0
│ │ │ │ │ │ └── extend-shallow@2.0.1
│ │ │ │ │ ├─┬ to-object-path@0.3.0
│ │ │ │ │ │ └── kind-of@3.2.2
│ │ │ │ │ ├─┬ union-value@1.0.0
│ │ │ │ │ │ └─┬ set-value@0.4.3
│ │ │ │ │ │   └── extend-shallow@2.0.1
│ │ │ │ │ └─┬ unset-value@1.0.0
│ │ │ │ │   └─┬ has-value@0.3.1
│ │ │ │ │     ├── has-values@0.1.4
│ │ │ │ │     └── isobject@2.1.0
│ │ │ │ ├─┬ class-utils@0.3.6
│ │ │ │ │ ├── arr-union@3.1.0
│ │ │ │ │ ├── define-property@0.2.5
│ │ │ │ │ └─┬ static-extend@0.1.2
│ │ │ │ │   ├── define-property@0.2.5
│ │ │ │ │   └─┬ object-copy@0.1.0
│ │ │ │ │     ├── copy-descriptor@0.1.1
│ │ │ │ │     ├── define-property@0.2.5
│ │ │ │ │     └── kind-of@3.2.2
│ │ │ │ ├─┬ define-property@1.0.0
│ │ │ │ │ └─┬ is-descriptor@1.0.2
│ │ │ │ │   ├── is-accessor-descriptor@1.0.0
│ │ │ │ │   └── is-data-descriptor@1.0.0
│ │ │ │ ├─┬ mixin-deep@1.3.1
│ │ │ │ │ ├── for-in@1.0.2
│ │ │ │ │ └── is-extendable@1.0.1
│ │ │ │ └── pascalcase@0.1.1
│ │ │ ├─┬ define-property@0.2.5
│ │ │ │ └─┬ is-descriptor@0.1.6
│ │ │ │   ├─┬ is-accessor-descriptor@0.1.6
│ │ │ │   │ └── kind-of@3.2.2
│ │ │ │   ├─┬ is-data-descriptor@0.1.4
│ │ │ │   │ └── kind-of@3.2.2
│ │ │ │   └── kind-of@5.1.0
│ │ │ ├── extend-shallow@2.0.1
│ │ │ ├── source-map@0.5.7
│ │ │ └── use@3.1.1
│ │ └── to-regex@3.0.2
│ ├─┬ mkdirp@0.5.1
│ │ └── minimist@0.0.8
│ └─┬ source-map-support@0.5.10
│   └── buffer-from@1.1.1
├── bootbox@4.4.0
├── bootstrap@3.4.1
├─┬ chai@3.5.0
│ ├── assertion-error@1.1.0
│ ├─┬ deep-eql@0.1.3
│ │ └── type-detect@0.1.1
│ └── type-detect@1.0.0
├── crypto-js@3.1.9-1 extraneous
├─┬ css@2.2.4
│ ├── inherits@2.0.3
│ ├── source-map@0.6.1
│ ├─┬ source-map-resolve@0.5.2
│ │ ├── atob@2.1.2
│ │ ├── decode-uri-component@0.2.0
│ │ ├── resolve-url@0.2.1
│ │ └── source-map-url@0.4.0
│ └── urix@0.1.0
├─┬ css-loader@0.25.0
│ ├─┬ babel-code-frame@6.26.0
│ │ ├─┬ chalk@1.1.3
│ │ │ ├── ansi-styles@2.2.1
│ │ │ ├── has-ansi@2.0.0
│ │ │ └── supports-color@2.0.0
│ │ ├── esutils@2.0.2
│ │ └── js-tokens@3.0.2
│ ├─┬ css-selector-tokenizer@0.6.0
│ │ ├── cssesc@0.1.0
│ │ └─┬ regexpu-core@1.0.0
│ │   ├── regenerate@1.4.0
│ │   ├── regjsgen@0.2.0
│ │   └─┬ regjsparser@0.1.5
│ │     └── jsesc@0.5.0
│ ├─┬ cssnano@3.10.0
│ │ ├─┬ autoprefixer@6.7.7
│ │ │ ├─┬ browserslist@1.7.7
│ │ │ │ └── electron-to-chromium@1.3.113
│ │ │ ├── caniuse-db@1.0.30000943
│ │ │ ├── normalize-range@0.1.2
│ │ │ └── num2fraction@1.2.2
│ │ ├── decamelize@1.2.0
│ │ ├── defined@1.0.0
│ │ ├─┬ has@1.0.3
│ │ │ └── function-bind@1.1.1
│ │ ├─┬ postcss-calc@5.3.1
│ │ │ ├── postcss-message-helpers@2.0.0
│ │ │ └─┬ reduce-css-calc@1.3.0
│ │ │   ├── balanced-match@0.4.2
│ │ │   ├── math-expression-evaluator@1.2.17
│ │ │   └─┬ reduce-function-call@1.0.2
│ │ │     └── balanced-match@0.4.2
│ │ ├─┬ postcss-colormin@2.2.2
│ │ │ └─┬ colormin@1.1.2
│ │ │   ├─┬ color@0.11.4
│ │ │   │ ├── clone@1.0.4
│ │ │   │ └── color-string@0.3.0
│ │ │   └── css-color-names@0.0.4
│ │ ├── postcss-convert-values@2.6.1
│ │ ├── postcss-discard-comments@2.0.4
│ │ ├── postcss-discard-duplicates@2.1.0
│ │ ├── postcss-discard-empty@2.1.0
│ │ ├── postcss-discard-overridden@0.1.1
│ │ ├─┬ postcss-discard-unused@2.2.3
│ │ │ └── uniqs@2.0.0
│ │ ├── postcss-filter-plugins@2.0.3
│ │ ├── postcss-merge-idents@2.1.7
│ │ ├── postcss-merge-longhand@2.0.2
│ │ ├─┬ postcss-merge-rules@2.1.2
│ │ │ ├─┬ caniuse-api@1.6.1
│ │ │ │ ├── lodash.memoize@4.1.2
│ │ │ │ └── lodash.uniq@4.5.0
│ │ │ ├─┬ postcss-selector-parser@2.2.3
│ │ │ │ ├── flatten@1.0.2
│ │ │ │ ├── indexes-of@1.0.1
│ │ │ │ └── uniq@1.0.1
│ │ │ └── vendors@1.0.2
│ │ ├── postcss-minify-font-values@1.0.5
│ │ ├── postcss-minify-gradients@1.0.5
│ │ ├─┬ postcss-minify-params@1.2.2
│ │ │ └── alphanum-sort@1.0.2
│ │ ├── postcss-minify-selectors@2.1.1
│ │ ├── postcss-normalize-charset@1.1.1
│ │ ├─┬ postcss-normalize-url@3.0.8
│ │ │ ├── is-absolute-url@2.1.0
│ │ │ └─┬ normalize-url@1.9.1
│ │ │   ├── prepend-http@1.0.4
│ │ │   ├─┬ query-string@4.3.4
│ │ │   │ └── strict-uri-encode@1.1.0
│ │ │   └─┬ sort-keys@1.1.2
│ │ │     └── is-plain-obj@1.1.0
│ │ ├── postcss-ordered-values@2.2.3
│ │ ├── postcss-reduce-idents@2.4.0
│ │ ├── postcss-reduce-initial@1.0.1
│ │ ├── postcss-reduce-transforms@1.0.4
│ │ ├─┬ postcss-svgo@2.1.6
│ │ │ ├─┬ is-svg@2.1.0
│ │ │ │ └── html-comment-regex@1.1.2
│ │ │ └─┬ svgo@0.7.2
│ │ │   ├─┬ coa@1.0.4
│ │ │   │ └── q@1.5.1
│ │ │   ├─┬ csso@2.3.2
│ │ │   │ ├─┬ clap@1.2.3
│ │ │   │ │ └─┬ chalk@1.1.3
│ │ │   │ │   ├── ansi-styles@2.2.1
│ │ │   │ │   └── supports-color@2.0.0
│ │ │   │ └── source-map@0.5.7
│ │ │   ├─┬ js-yaml@3.7.0
│ │ │   │ ├─┬ argparse@1.0.10
│ │ │   │ │ └── sprintf-js@1.0.3
│ │ │   │ └── esprima@2.7.3
│ │ │   ├── sax@1.2.4
│ │ │   └── whet.extend@0.9.9
│ │ ├── postcss-unique-selectors@2.0.2
│ │ ├── postcss-value-parser@3.3.1
│ │ └── postcss-zindex@2.2.0
│ ├─┬ lodash.camelcase@3.0.1
│ │ └─┬ lodash._createcompounder@3.0.0
│ │   ├─┬ lodash.deburr@3.2.0
│ │   │ └── lodash._root@3.0.1
│ │   └── lodash.words@3.2.0
│ ├── object-assign@4.1.1
│ ├─┬ postcss@5.2.18
│ │ ├─┬ chalk@1.1.3
│ │ │ ├── ansi-styles@2.2.1
│ │ │ └── supports-color@2.0.0
│ │ ├── js-base64@2.5.1
│ │ ├── source-map@0.5.7
│ │ └─┬ supports-color@3.2.3
│ │   └── has-flag@1.0.0
│ ├─┬ postcss-modules-extract-imports@1.2.1
│ │ └── postcss@6.0.23
│ ├─┬ postcss-modules-local-by-default@1.2.0
│ │ ├── css-selector-tokenizer@0.7.1
│ │ └── postcss@6.0.23
│ ├─┬ postcss-modules-scope@1.1.0
│ │ ├── css-selector-tokenizer@0.7.1
│ │ └── postcss@6.0.23
│ ├─┬ postcss-modules-values@1.3.0
│ │ ├── icss-replace-symbols@1.1.0
│ │ └── postcss@6.0.23
│ └── source-list-map@0.1.8
├── es6-shim@0.35.5
├── event-source-polyfill@0.0.7
├── expose-loader@0.7.5
├─┬ extract-text-webpack-plugin@2.1.2
│ ├── async@2.6.2
│ ├─┬ loader-utils@1.2.3
│ │ ├── big.js@5.2.2
│ │ └─┬ json5@1.0.1
│ │   └── minimist@1.2.0
│ ├─┬ schema-utils@0.3.0
│ │ └─┬ ajv@5.5.2
│ │   ├── fast-deep-equal@1.1.0
│ │   ├── fast-json-stable-stringify@2.0.0
│ │   └── json-schema-traverse@0.3.1
│ └─┬ webpack-sources@1.3.0
│   └── source-list-map@2.0.1
├── file-loader@0.9.0
├── font-awesome@4.7.0
├─┬ html-loader@0.4.5
│ ├─┬ es6-templates@0.2.3
│ │ ├─┬ recast@0.11.23
│ │ │ ├── ast-types@0.9.6
│ │ │ ├── esprima@3.1.3
│ │ │ ├── private@0.1.8
│ │ │ └── source-map@0.5.7
│ │ └── through@2.3.8
│ ├── fastparse@1.1.2
│ ├─┬ html-minifier@3.5.21
│ │ ├─┬ camel-case@3.0.0
│ │ │ ├─┬ no-case@2.3.2
│ │ │ │ └── lower-case@1.1.4
│ │ │ └── upper-case@1.1.3
│ │ ├── clean-css@4.2.1
│ │ ├── commander@2.17.1
│ │ ├── he@1.2.0
│ │ ├── param-case@2.1.1
│ │ ├── relateurl@0.2.7
│ │ └─┬ uglify-js@3.4.9
│ │   └── commander@2.17.1
│ └─┬ loader-utils@1.2.3
│   ├── big.js@5.2.2
│   └─┬ json5@1.0.1
│     └── minimist@1.2.0
├─┬ isomorphic-fetch@2.2.1
│ ├─┬ node-fetch@1.7.3
│ │ ├─┬ encoding@0.1.12
│ │ │ └── iconv-lite@0.4.24
│ │ └── is-stream@1.1.0
│ └── whatwg-fetch@3.0.0
├── jasmine-core@2.99.1
├── jquery@2.2.4
├── jquery-validation@1.19.0
├── jquery-validation-unobtrusive@3.2.11
├── json-loader@0.5.7
├─┬ karma@1.7.1
│ ├── bluebird@3.5.3
│ ├─┬ body-parser@1.18.3
│ │ ├── bytes@3.0.0
│ │ ├── content-type@1.0.4
│ │ ├── depd@1.1.2
│ │ ├─┬ http-errors@1.6.3
│ │ │ ├── setprototypeof@1.1.0
│ │ │ └── statuses@1.5.0
│ │ ├─┬ iconv-lite@0.4.23
│ │ │ └── safer-buffer@2.1.2
│ │ ├─┬ on-finished@2.3.0
│ │ │ └── ee-first@1.1.1
│ │ ├── qs@6.5.2
│ │ ├─┬ raw-body@2.3.3
│ │ │ └── iconv-lite@0.4.23
│ │ └─┬ type-is@1.6.16
│ │   ├── media-typer@0.3.0
│ │   └─┬ mime-types@2.1.22
│ │     └── mime-db@1.38.0
│ ├─┬ chokidar@1.7.0
│ │ ├─┬ anymatch@1.3.2
│ │ │ ├─┬ micromatch@2.3.11
│ │ │ │ ├── arr-diff@2.0.0
│ │ │ │ ├── array-unique@0.2.1
│ │ │ │ ├─┬ braces@1.8.5
│ │ │ │ │ ├─┬ expand-range@1.8.2
│ │ │ │ │ │ └─┬ fill-range@2.2.4
│ │ │ │ │ │   ├─┬ is-number@2.1.0
│ │ │ │ │ │   │ └── kind-of@3.2.2
│ │ │ │ │ │   ├── isobject@2.1.0
│ │ │ │ │ │   └─┬ randomatic@3.1.1
│ │ │ │ │ │     ├── is-number@4.0.0
│ │ │ │ │ │     └── math-random@1.0.4
│ │ │ │ │ └── preserve@0.2.0
│ │ │ │ ├─┬ expand-brackets@0.1.5
│ │ │ │ │ └── is-posix-bracket@0.1.1
│ │ │ │ ├── extglob@0.3.2
│ │ │ │ ├── filename-regex@2.0.1
│ │ │ │ ├─┬ kind-of@3.2.2
│ │ │ │ │ └── is-buffer@1.1.6
│ │ │ │ ├─┬ object.omit@2.0.1
│ │ │ │ │ └── for-own@0.1.5
│ │ │ │ ├─┬ parse-glob@3.0.4
│ │ │ │ │ ├─┬ glob-base@0.3.0
│ │ │ │ │ │ ├── glob-parent@2.0.0
│ │ │ │ │ │ └─┬ is-glob@2.0.1
│ │ │ │ │ │   └── is-extglob@1.0.0
│ │ │ │ │ ├── is-dotfile@1.0.3
│ │ │ │ │ ├── is-extglob@1.0.0
│ │ │ │ │ └── is-glob@2.0.1
│ │ │ │ └─┬ regex-cache@0.4.4
│ │ │ │   └─┬ is-equal-shallow@0.1.3
│ │ │ │     └── is-primitive@2.0.0
│ │ │ └─┬ normalize-path@2.1.1
│ │ │   └── remove-trailing-separator@1.1.0
│ │ ├── async-each@1.0.1
│ │ ├── UNMET OPTIONAL DEPENDENCY fsevents@^1.0.0
│ │ ├── glob-parent@2.0.0
│ │ ├─┬ is-binary-path@1.0.1
│ │ │ └── binary-extensions@1.13.0
│ │ ├─┬ is-glob@2.0.1
│ │ │ └── is-extglob@1.0.0
│ │ └── readdirp@2.2.1
│ ├── colors@1.1.2
│ ├── combine-lists@1.0.1
│ ├── core-js@2.6.5
│ ├── di@0.0.1
│ ├─┬ dom-serialize@2.2.1
│ │ ├── custom-event@1.0.1
│ │ ├── ent@2.2.0
│ │ ├── extend@3.0.2
│ │ └── void-elements@2.0.1
│ ├─┬ expand-braces@0.1.2
│ │ ├── array-slice@0.2.3
│ │ ├── array-unique@0.2.1
│ │ └─┬ braces@0.1.5
│ │   └─┬ expand-range@0.1.1
│ │     ├── is-number@0.1.1
│ │     └── repeat-string@0.2.2
│ ├─┬ glob@7.1.3
│ │ ├── fs.realpath@1.0.0
│ │ ├─┬ inflight@1.0.6
│ │ │ └── wrappy@1.0.2
│ │ └── once@1.4.0
│ ├── graceful-fs@4.1.15
│ ├─┬ http-proxy@1.17.0
│ │ ├── eventemitter3@3.1.0
│ │ ├─┬ follow-redirects@1.7.0
│ │ │ └─┬ debug@3.2.6
│ │ │   └── ms@2.1.1
│ │ └── requires-port@1.0.0
│ ├─┬ isbinaryfile@3.0.3
│ │ └─┬ buffer-alloc@1.2.0
│ │   ├── buffer-alloc-unsafe@1.1.0
│ │   └── buffer-fill@1.0.0
│ ├── lodash@3.10.1
│ ├─┬ log4js@0.6.38
│ │ ├─┬ readable-stream@1.0.34
│ │ │ ├── isarray@0.0.1
│ │ │ └── string_decoder@0.10.31
│ │ └── semver@4.3.6
│ ├── mime@1.6.0
│ ├─┬ minimatch@3.0.4
│ │ └─┬ brace-expansion@1.1.11
│ │   ├── balanced-match@1.0.0
│ │   └── concat-map@0.0.1
│ ├─┬ optimist@0.6.1
│ │ └── wordwrap@0.0.2
│ ├── qjobs@1.2.0
│ ├── range-parser@1.2.0
│ ├── rimraf@2.6.3
│ ├── safe-buffer@5.1.2
│ ├─┬ socket.io@1.7.3
│ │ ├─┬ debug@2.3.3
│ │ │ └── ms@0.7.2
│ │ ├─┬ engine.io@1.8.3
│ │ │ ├─┬ accepts@1.3.3
│ │ │ │ └── negotiator@0.6.1
│ │ │ ├── base64id@1.0.0
│ │ │ ├── cookie@0.3.1
│ │ │ ├─┬ debug@2.3.3
│ │ │ │ └── ms@0.7.2
│ │ │ ├─┬ engine.io-parser@1.3.2
│ │ │ │ ├── after@0.8.2
│ │ │ │ ├── arraybuffer.slice@0.0.6
│ │ │ │ ├── base64-arraybuffer@0.1.5
│ │ │ │ ├── blob@0.0.4
│ │ │ │ └── wtf-8@1.0.0
│ │ │ └─┬ ws@1.1.2
│ │ │   ├── options@0.0.6
│ │ │   └── ultron@1.0.2
│ │ ├─┬ has-binary@0.1.7
│ │ │ └── isarray@0.0.1
│ │ ├── object-assign@4.1.0
│ │ ├─┬ socket.io-adapter@0.5.0
│ │ │ └─┬ debug@2.3.3
│ │ │   └── ms@0.7.2
│ │ ├─┬ socket.io-client@1.7.3
│ │ │ ├── backo2@1.0.2
│ │ │ ├── component-bind@1.0.0
│ │ │ ├── component-emitter@1.2.1
│ │ │ ├─┬ debug@2.3.3
│ │ │ │ └── ms@0.7.2
│ │ │ ├─┬ engine.io-client@1.8.3
│ │ │ │ ├── component-inherit@0.0.3
│ │ │ │ ├─┬ debug@2.3.3
│ │ │ │ │ └── ms@0.7.2
│ │ │ │ ├── has-cors@1.1.0
│ │ │ │ ├── parsejson@0.0.3
│ │ │ │ ├── parseqs@0.0.5
│ │ │ │ ├── xmlhttprequest-ssl@1.5.3
│ │ │ │ └── yeast@0.1.2
│ │ │ ├── indexof@0.0.1
│ │ │ ├── object-component@0.0.3
│ │ │ ├─┬ parseuri@0.0.5
│ │ │ │ └─┬ better-assert@1.0.2
│ │ │ │   └── callsite@1.0.0
│ │ │ └── to-array@0.1.4
│ │ └─┬ socket.io-parser@2.3.1
│ │   ├── component-emitter@1.1.2
│ │   ├─┬ debug@2.2.0
│ │   │ └── ms@0.7.1
│ │   ├── isarray@0.0.1
│ │   └── json3@3.3.2
│ ├── source-map@0.5.7
│ ├─┬ tmp@0.0.31
│ │ └── os-tmpdir@1.0.2
│ └─┬ useragent@2.3.0
│   └─┬ lru-cache@4.1.5
│     ├── pseudomap@1.0.2
│     └── yallist@2.1.2
├── karma-chai@0.1.0
├─┬ karma-chrome-launcher@2.2.0
│ ├─┬ fs-access@1.0.1
│ │ └── null-check@1.0.0
│ └─┬ which@1.3.1
│   └── isexe@2.0.0
├─┬ karma-cli@1.0.1
│ └─┬ resolve@1.10.0
│   └── path-parse@1.0.6
├── karma-jasmine@1.1.2
├─┬ karma-webpack@1.8.1
│ ├── async@0.9.2
│ ├── lodash@3.10.1
│ └─┬ source-map@0.1.43
│   └── amdefine@1.0.1
├── ng2-toasty@4.0.3
├── preboot@4.5.2
├── raw-loader@0.5.1
├─┬ UNMET PEER DEPENDENCY rxjs@5.5.12
│ └── symbol-observable@1.0.1
├─┬ style-loader@0.13.2
│ └─┬ loader-utils@1.2.3
│   ├── big.js@5.2.2
│   └─┬ json5@1.0.1
│     └── minimist@1.2.0
├── to-string-loader@1.1.5
├── typeahead.js@0.11.1
├── typescript@2.9.2
├── underscore@1.9.1
├─┬ url-loader@0.5.9
│ ├─┬ loader-utils@1.2.3
│ │ ├── big.js@5.2.2
│ │ └─┬ json5@1.0.1
│ │   └── minimist@1.2.0
│ └── mime@1.3.6
├─┬ webpack@2.7.0
│ ├── acorn@5.7.3
│ ├─┬ acorn-dynamic-import@2.0.2
│ │ └── acorn@4.0.13
│ ├─┬ ajv@4.11.8
│ │ ├── co@4.6.0
│ │ └─┬ json-stable-stringify@1.0.1
│ │   └── jsonify@0.0.0
│ ├── ajv-keywords@1.5.1
│ ├── interpret@1.2.0
│ ├── json5@0.5.1
│ ├── loader-runner@2.4.0
│ ├── memory-fs@0.4.1
│ ├─┬ node-libs-browser@2.2.0
│ │ ├─┬ assert@1.4.1
│ │ │ └─┬ util@0.10.3
│ │ │   └── inherits@2.0.1
│ │ ├─┬ browserify-zlib@0.2.0
│ │ │ └── pako@1.0.10
│ │ ├─┬ buffer@4.9.1
│ │ │ ├── base64-js@1.3.0
│ │ │ └── ieee754@1.1.12
│ │ ├─┬ console-browserify@1.1.0
│ │ │ └── date-now@0.1.4
│ │ ├── constants-browserify@1.0.0
│ │ ├─┬ crypto-browserify@3.12.0
│ │ │ ├─┬ browserify-cipher@1.0.1
│ │ │ │ ├─┬ browserify-aes@1.2.0
│ │ │ │ │ └── buffer-xor@1.0.3
│ │ │ │ ├─┬ browserify-des@1.0.2
│ │ │ │ │ └── des.js@1.0.0
│ │ │ │ └── evp_bytestokey@1.0.3
│ │ │ ├─┬ browserify-sign@4.0.4
│ │ │ │ ├── bn.js@4.11.8
│ │ │ │ ├── browserify-rsa@4.0.1
│ │ │ │ ├─┬ elliptic@6.4.1
│ │ │ │ │ ├── brorand@1.1.0
│ │ │ │ │ ├── hash.js@1.1.7
│ │ │ │ │ ├── hmac-drbg@1.0.1
│ │ │ │ │ ├── minimalistic-assert@1.0.1
│ │ │ │ │ └── minimalistic-crypto-utils@1.0.1
│ │ │ │ └─┬ parse-asn1@5.1.4
│ │ │ │   └── asn1.js@4.10.1
│ │ │ ├── create-ecdh@4.0.3
│ │ │ ├─┬ create-hash@1.2.0
│ │ │ │ ├── cipher-base@1.0.4
│ │ │ │ ├─┬ md5.js@1.3.5
│ │ │ │ │ └── hash-base@3.0.4
│ │ │ │ ├── ripemd160@2.0.2
│ │ │ │ └── sha.js@2.4.11
│ │ │ ├── create-hmac@1.1.7
│ │ │ ├─┬ diffie-hellman@5.0.3
│ │ │ │ └── miller-rabin@4.0.1
│ │ │ ├── pbkdf2@3.0.17
│ │ │ ├── public-encrypt@4.0.3
│ │ │ ├── randombytes@2.1.0
│ │ │ └── randomfill@1.0.4
│ │ ├── domain-browser@1.2.0
│ │ ├── events@3.0.0
│ │ ├── https-browserify@1.0.0
│ │ ├── os-browserify@0.3.0
│ │ ├── path-browserify@0.0.0
│ │ ├── process@0.11.10
│ │ ├── punycode@1.4.1
│ │ ├── querystring-es3@0.2.1
│ │ ├── stream-browserify@2.0.2
│ │ ├─┬ stream-http@2.8.3
│ │ │ ├── builtin-status-codes@3.0.0
│ │ │ ├── to-arraybuffer@1.0.1
│ │ │ └── xtend@4.0.1
│ │ ├── string_decoder@1.1.1
│ │ ├─┬ timers-browserify@2.0.10
│ │ │ └── setimmediate@1.0.5
│ │ ├── tty-browserify@0.0.0
│ │ ├─┬ url@0.11.0
│ │ │ └── punycode@1.3.2
│ │ ├── util@0.11.1
│ │ └── vm-browserify@0.0.4
│ ├── source-map@0.5.7
│ ├─┬ supports-color@3.2.3
│ │ └── has-flag@1.0.0
│ ├── tapable@0.2.9
│ ├─┬ uglify-js@2.8.29
│ │ ├── uglify-to-browserify@1.0.2
│ │ └─┬ yargs@3.10.0
│ │   ├── camelcase@1.2.1
│ │   ├─┬ cliui@2.1.0
│ │   │ ├─┬ center-align@0.1.3
│ │   │ │ ├─┬ align-text@0.1.4
│ │   │ │ │ ├── kind-of@3.2.2
│ │   │ │ │ └── longest@1.0.1
│ │   │ │ └── lazy-cache@1.0.4
│ │   │ └── right-align@0.1.3
│ │   └── window-size@0.1.0
│ ├─┬ watchpack@1.6.0
│ │ ├─┬ chokidar@2.1.2
│ │ │ ├─┬ anymatch@2.0.0
│ │ │ │ └── normalize-path@2.1.1
│ │ │ ├── UNMET OPTIONAL DEPENDENCY fsevents@^1.2.7
│ │ │ ├─┬ glob-parent@3.1.0
│ │ │ │ ├── is-glob@3.1.0
│ │ │ │ └── path-dirname@1.0.2
│ │ │ ├─┬ is-glob@4.0.0
│ │ │ │ └── is-extglob@2.1.1
│ │ │ ├── normalize-path@3.0.0
│ │ │ └── upath@1.1.1
│ │ └── neo-async@2.6.0
│ └─┬ yargs@6.6.0
│   ├── camelcase@3.0.0
│   ├─┬ cliui@3.2.0
│   │ └── wrap-ansi@2.1.0
│   ├── get-caller-file@1.0.3
│   ├─┬ os-locale@1.4.0
│   │ └─┬ lcid@1.0.0
│   │   └── invert-kv@1.0.0
│   ├─┬ read-pkg-up@1.0.1
│   │ ├─┬ find-up@1.1.2
│   │ │ ├── path-exists@2.1.0
│   │ │ └─┬ pinkie-promise@2.0.1
│   │ │   └── pinkie@2.0.4
│   │ └─┬ read-pkg@1.1.0
│   │   ├─┬ load-json-file@1.1.0
│   │   │ ├─┬ parse-json@2.2.0
│   │   │ │ └─┬ error-ex@1.3.2
│   │   │ │   └── is-arrayish@0.2.1
│   │   │ ├── pify@2.3.0
│   │   │ └─┬ strip-bom@2.0.0
│   │   │   └── is-utf8@0.2.1
│   │   ├─┬ normalize-package-data@2.5.0
│   │   │ ├── hosted-git-info@2.7.1
│   │   │ └─┬ validate-npm-package-license@3.0.4
│   │   │   ├─┬ spdx-correct@3.1.0
│   │   │   │ └── spdx-license-ids@3.0.3
│   │   │   └─┬ spdx-expression-parse@3.0.0
│   │   │     └── spdx-exceptions@2.2.0
│   │   └── path-type@1.1.0
│   ├── require-directory@2.1.1
│   ├── require-main-filename@1.0.1
│   ├── set-blocking@2.0.0
│   ├─┬ string-width@1.0.2
│   │ ├── code-point-at@1.1.0
│   │ └─┬ is-fullwidth-code-point@1.0.0
│   │   └── number-is-nan@1.0.1
│   ├── which-module@1.0.0
│   ├── y18n@3.2.1
│   └─┬ yargs-parser@4.2.1
│     └── camelcase@3.0.0
├─┬ webpack-hot-middleware@2.24.3
│ ├── ansi-html@0.0.7
│ ├── html-entities@1.2.1
│ ├── querystring@0.2.0
│ └─┬ strip-ansi@3.0.1
│   └── ansi-regex@2.1.1
├─┬ webpack-merge@0.14.1
│ ├─┬ lodash.find@3.2.1
│ │ ├─┬ lodash._basecallback@3.3.1
│ │ │ ├── lodash._baseisequal@3.0.7
│ │ │ ├── lodash._bindcallback@3.0.1
│ │ │ └── lodash.pairs@3.0.1
│ │ ├── lodash._baseeach@3.0.4
│ │ ├── lodash._basefind@3.0.0
│ │ ├── lodash._basefindindex@3.6.0
│ │ ├── lodash.isarray@3.0.4
│ │ └── lodash.keys@3.1.2
│ ├── lodash.isequal@4.5.0
│ ├─┬ lodash.isplainobject@3.2.0
│ │ ├── lodash._basefor@3.0.3
│ │ ├── lodash.isarguments@3.1.0
│ │ └── lodash.keysin@3.0.8
│ └─┬ lodash.merge@3.3.2
│   ├── lodash._arraycopy@3.0.0
│   ├── lodash._arrayeach@3.0.0
│   ├─┬ lodash._createassigner@3.1.1
│   │ ├── lodash._isiterateecall@3.0.9
│   │ └── lodash.restparam@3.6.1
│   ├── lodash._getnative@3.9.1
│   ├── lodash.istypedarray@3.0.6
│   └─┬ lodash.toplainobject@3.0.0
│     └── lodash._basecopy@3.0.1
└── UNMET PEER DEPENDENCY zone.js@0.7.8
```
