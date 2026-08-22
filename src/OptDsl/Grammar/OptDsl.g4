grammar OptDsl;

// --- Parser rules ---

// Root
translation_unit: optimization_block+ EOF;

// List of statements
optimization_block:
	OPT STRING OPEN_CBRKT statement_list CLOSE_CBRKT;
statement_list: statement+;
statement: optimization_statement DELIMITER;
optimization_statement: optimization parameter EQL value;

// Rules for lexeme recognition 
optimization: INLINE | FOLD | LOOP | CFG;
parameter: THRESHOLD | ITERATION;
value: INT | BOOL;

// --- Lexer rules ---

// Special characters
DELIMITER: ';';
OPEN_CBRKT: '{';
CLOSE_CBRKT: '}';
EQL: '=';
// COMMA: ','; // Maybe will not use

// Optimization keywords
OPT: 'optimize';
INLINE: 'inline';
FOLD: 'constant_fold';
LOOP: 'loop_unroll';
CFG: 'simplifycfg';

// Parameter keywords
THRESHOLD: 'threshold';
ITERATION: 'max_iterations';

// Basic lexemes
STRING: '"' ~["\r\n]* '"';
ID: [_A-Za-z][_A-Za-z0-9]*;
INT: [0-9]+;
BOOL: 'true' | 'false';
WS: [ \t\r\n]+ -> skip;
