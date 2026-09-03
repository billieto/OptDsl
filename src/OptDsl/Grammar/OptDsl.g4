grammar OptDsl;

// === Parser rules ===

// Root
translation_unit: optimization_block+ EOF;

// List of statements
optimization_block: OPT STRING OPEN_CBRKT statement+ CLOSE_CBRKT;

statement
	: ID parameter_list? DELIMITER # OptimizationStatement
	| scope OPEN_CBRKT statement+ CLOSE_CBRKT # ScopeBlock
	;

parameter_list: param_assignment (COMMA param_assignment)*;
param_assignment: ID EQL value;

// Rules for lexeme recognition 
// optimization: INLINE | FOLD | LOOP | CFG;
// parameter: THRESHOLD | ITERATION;
value: INT | BOOL;
scope: MODULE | FUNCTION | LOOP;

// === Lexer rules ===

// Special characters
DELIMITER: ';';
OPEN_CBRKT: '{';
CLOSE_CBRKT: '}';
EQL: '=';
COMMA: ',';

// Scope keywords
MODULE: 'module';
FUNCTION: 'function';
LOOP: 'loop';

// Optimization keywords
OPT: 'optimize';
// INLINE: 'inline';
// FOLD: 'constant_fold';
// LOOP: 'loop_unroll';
// CFG: 'simplifycfg';

// // Parameter keywords
// THRESHOLD: 'threshold';
// ITERATION: 'max_iterations';

// Basic lexemes
BOOL: 'true' | 'false';
INT: [0-9]+;
STRING: '"' ~["\r\n]* '"';
ID: [_A-Za-z][_A-Za-z0-9]*;
WS: [ \t\r\n]+ -> skip;
