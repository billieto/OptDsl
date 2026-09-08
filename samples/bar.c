#include <stdio.h>

int bar(void)
{
    int sum = 0;
    
    // A simple loop that can be unrolled
    for (int i = 0; i < 4; i++) {
        sum += i;
    }

    // A branch that simplifycfg can optimize
    if (sum > 0) {
        printf("Sum is positive: %d\n", sum);
    } else {
        printf("Sum is zero or negative: %d\n", sum);
    }

    return 0;
}
