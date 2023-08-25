/* eslint-env node */
module.exports = {
  root: true,

  extends: [
    'plugin:vue/vue3-essential',
    'eslint:recommended'
  ],

  parserOptions: {
    ecmaVersion: 'latest'
  },

  rules: {
    'block-spacing': 'warn',
    'comma-dangle': ['warn', 'never'],
    'comma-spacing': 'warn',
    'eol-last': ['warn', 'never'],
    'indent': ['warn', 2, { 'SwitchCase': 1 }],
    'jsx-quotes': ['warn', 'prefer-single'],
    'key-spacing': 'warn',
    'keyword-spacing': 'warn',
    'no-multiple-empty-lines': ['warn', { 'max': 1 }],
    'no-trailing-spaces': 'warn',
    'object-curly-newline': ['warn', { 'consistent': true }],
    'operator-assignment': ['warn', 'always'],
    'no-console': 'warn',
    'no-debugger': 'warn',
    'no-unused-vars': 1,
    'no-whitespace-before-property': 'warn',
    'padded-blocks': ['warn', { 'blocks': 'never' }],
    'semi': ['warn', 'never'],
    'space-before-blocks': 'warn',
    'space-before-function-paren': [1, 'never'],
    'space-infix-ops': 'warn',
    'vue/html-self-closing': 0,
    'vue/multi-word-component-names': 'off',
    'padding-line-between-statements': [
      'warn',
      { blankLine: 'always', prev: '*', next: ['block-like', 'class', 'export'] },
      { blankLine: 'any', prev: 'export', next: 'export' }
    ]
  }
}