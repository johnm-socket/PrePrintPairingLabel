#!/usr/bin/env python3
"""Generate app.ico for PrePrint Pairing Label."""
import struct, zlib

def png(w, h, rows_rgba):
    def chunk(tag, data):
        crc = zlib.crc32(tag + data) & 0xffffffff
        return struct.pack('>I', len(data)) + tag + data + struct.pack('>I', crc)
    ihdr = struct.pack('>IIBBBBB', w, h, 8, 6, 0, 0, 0)
    raw  = b''.join(b'\x00' + bytes(p for px in row for p in px) for row in rows_rgba)
    return b'\x89PNG\r\n\x1a\n' + chunk(b'IHDR', ihdr) + chunk(b'IDAT', zlib.compress(raw)) + chunk(b'IEND', b'')

def icon_pixels(n):
    B = (0x1E, 0x88, 0xE5, 0xFF)   # #1E88E5 blue
    W = (0xFF, 0xFF, 0xFF, 0xFF)   # white
    K = (0x0D, 0x47, 0xA1, 0xFF)   # dark blue  #0D47A1
    G = (0xE3, 0xF2, 0xFD, 0xFF)   # light blue tint

    m  = max(2, n // 8)            # border width
    # label rect inside border
    lx1 = m + m // 2; lx2 = n - lx1
    ly1 = m + m // 2; ly2 = n - ly1
    lw  = lx2 - lx1;  lh  = ly2 - ly1

    # barcode stripes (bottom 45% of label area)
    bc_y = ly1 + int(lh * 0.55)
    # 7-stripe pattern that looks like a barcode
    pattern = [1,1,0,1,0,0,1,1,0,1,0,1,1,0,1,1,0,0,1,0,1,0,1,1,0,1,0,0,1]

    def pixel(x, y):
        # Outer background
        if x < m or x >= n-m or y < m or y >= n-m:
            return B
        # Label rectangle
        if lx1 <= x < lx2 and ly1 <= y < ly2:
            if y >= bc_y:
                # Barcode stripes
                rel = x - lx1
                idx = int(rel * len(pattern) / lw)
                return K if pattern[idx % len(pattern)] else W
            elif y < ly1 + lh // 4:
                # Top of label: two bold "P" bumps represented as dark rectangles
                # Left "P" stem
                if lx1 + 1 <= x <= lx1 + lw//10 + 1:
                    return K
                # Left "P" bump
                if lx1 + 1 <= x <= lx1 + lw//4 and ly1 + 2 <= y <= ly1 + lh // 8:
                    return K
                # Right "P" stem
                if lx1 + lw//2 <= x <= lx1 + lw//2 + lw//10 + 1:
                    return K
                # Right "P" bump
                if lx1 + lw//2 <= x <= lx1 + lw//2 + lw//4 and ly1 + 2 <= y <= ly1 + lh // 8:
                    return K
                return W
            else:
                return G
        return B

    return [[pixel(x, y) for x in range(n)] for y in range(n)]

# Build two sizes
sizes = [16, 32]
pngs  = [png(s, s, icon_pixels(s)) for s in sizes]

# ICO header
header  = struct.pack('<HHH', 0, 1, len(sizes))
offset  = 6 + 16 * len(sizes)
entries = b''
for s, p in zip(sizes, pngs):
    entries += struct.pack('<BBBBHHII', s, s, 0, 0, 1, 32, len(p), offset)
    offset  += len(p)

out = 'app.ico'
with open(out, 'wb') as f:
    f.write(header + entries + b''.join(pngs))
print(f'Created {out} ({sum(len(p) for p in pngs)} bytes of image data)')
