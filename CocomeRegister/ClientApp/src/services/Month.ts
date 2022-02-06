export enum Month {
    JANUARY = 1,
    FEBRUARY,
    MARCH,
    APRIL,
    MAY,
    JUNE,
    JULY,
    AUGUST,
    SEPTEMBER,
    OCTOBER,
    NOVEMBER,
    DECEMBER
}

export const monthOrdinals = Object.keys(Month).filter(m => !isNaN(Number(m)));

export const monthValues = Object.keys(Month)
    .filter(m => isNaN(Number(m)))
    .map(m => m.charAt(0).toUpperCase() + m.slice(1).toLowerCase());
