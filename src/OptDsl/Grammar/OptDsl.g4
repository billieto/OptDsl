grammar OptDsl;

// --- Parser rules ---

// Root
translation_unit: optimization_block+ EOF;

// List of statements
optimization_block:
	OPT ID OPEN_CBRKT statement_list CLOSE_CBRKT;

statement_list: statement+;
statement: optimization_statement DELIMITER;

optimization_statement: optimization parameter EQL INT;

// Rules for lexeme recognition 
optimization: INLINE | FOLD | LOOP;

parameter: THRESHOLD;

// --- Lexer rules ---

// Special characters
DELIMITER: ';';
OPEN_CBRKT: '{';
CLOSE_CBRKT: '}';
EQL: '=';
COMMA: ','; // Maybe will not use

// Optimization keywords
OPT: 'optimize';
INLINE: 'inline';
FOLD: 'constant_fold';
LOOP: 'loop_unroll';

// Parameter keywords
THRESHOLD: 'threshold';

// Basic lexemes
ID: [a-z]+;
INT: [0-9]+;
BOOL: 'true' | 'false';
WS: [ \t\r\n]+ -> skip;
