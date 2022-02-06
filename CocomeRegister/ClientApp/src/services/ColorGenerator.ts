import { interpolateInferno } from 'd3';

export interface ColorRange {
    colorStart: number;
    colorEnd: number;
}

export function toRGBA(hex: string, alpha: number) {
    const r = parseInt(hex.slice(1, 3), 16);
    const g = parseInt(hex.slice(3, 5), 16);
    const b = parseInt(hex.slice(5, 7), 16);
    return `rgba(${r}, ${g}, ${b}, ${alpha})`;
}

function calculatePoint(index: number, intervalSize: number, colorRange: ColorRange) {
    return (colorRange.colorEnd - (index * intervalSize));
}

export function interpolateColors(dataLength: number, colorRange: ColorRange) {
    const intervalSize = (colorRange.colorEnd - colorRange.colorStart) / dataLength;
    const colorArray = [];
    let colorPoint: number;

    for (let i = 0; i < dataLength; i++) {
      colorPoint = calculatePoint(i, intervalSize, colorRange);
      colorArray.push(interpolateInferno(colorPoint));
    }
    return colorArray;
}
