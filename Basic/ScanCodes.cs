using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace FoenixIDE.Basic
{
    public enum ScanCode
    {
        sc_null = 0x00,
        sc_escape = 0x01,
        sc_1 = 0x02,
        sc_2 = 0x03,
        sc_3 = 0x04,
        sc_4 = 0x05,
        sc_5 = 0x06,
        sc_6 = 0x07,
        sc_7 = 0x08,
        sc_8 = 0x09,
        sc_9 = 0x0A,
        sc_0 = 0x0B,
        sc_minus = 0x0C,
        sc_equals = 0x0D,
        sc_backspace = 0x0E,
        sc_tab = 0x0F,
        sc_q = 0x10,
        sc_w = 0x11,
        sc_e = 0x12,
        sc_r = 0x13,
        sc_t = 0x14,
        sc_y = 0x15,
        sc_u = 0x16,
        sc_i = 0x17,
        sc_o = 0x18,
        sc_p = 0x19,
        sc_bracketLeft = 0x1A,
        sc_bracketRight = 0x1B,
        sc_enter = 0x1C,
        sc_controlLeft = 0x1D,
        sc_a = 0x1E,
        sc_s =0x1F,
        sc_d = 0x20,
        sc_f = 0x21,
        sc_g = 0x22,
        sc_h = 0x23,
        sc_j = 0x24,
        sc_k = 0x25,
        sc_l = 0x26,
        sc_semicolon = 0x27,
        sc_apostrophe = 0x28,
        sc_grave = 0x29,
        sc_shiftLeft = 0x2A,
        sc_backslash = 0x2B,
        sc_z = 0x2C,
        sc_x = 0x2D,
        sc_c = 0x2E,
        sc_v = 0x2F,
        sc_b = 0x30,
        sc_n = 0x31,
        sc_m = 0x32,
        sc_comma = 0x33,
        sc_period = 0x34,
        sc_slash = 0x35,
        sc_shiftRight = 0x36,
        sc_numpad_multiply = 0x37,
        sc_altLeft = 0x38,
        sc_space = 0x39,
        sc_capslock = 0x3A,
        sc_F1 = 0x3B,
        sc_F2 = 0x3C,
        sc_F3 = 0x3D,
        sc_F4 = 0x3E,
        sc_F5 = 0x3F,
        sc_F6 = 0x40,
        sc_F7 = 0x41,
        sc_F8 = 0x42,
        sc_F9 = 0x43,
        sc_F10 = 0x44,
        sc_F11 = 0x57,
        sc_F12 = 0x58,
        sc_up_arrow = 0x48,    // also maps to num keypad 8
        sc_left_arrow = 0x4B,  // also maps to num keypad 4
        sc_right_arrow = 0x4D, // also maps to num keypad 6
        sc_down_arrow = 0x50   // also maps to num keypad 2
    }

    class ScanCodes
    {
        public static ScanCode GetScanCode(Gdk.Key key)
        {
            return key switch {
                Gdk.Key.Key_0 => ScanCode.sc_0,
                Gdk.Key.Key_1 or Gdk.Key.Key_2 or Gdk.Key.Key_3 or
                Gdk.Key.Key_4 or Gdk.Key.Key_5 or Gdk.Key.Key_6 or
                Gdk.Key.Key_7 or Gdk.Key.Key_8 or Gdk.Key.Key_9 => ScanCode.sc_1 + (key - Gdk.Key.Key_1),
                Gdk.Key.A or Gdk.Key.a => ScanCode.sc_a,
                Gdk.Key.B or Gdk.Key.b => ScanCode.sc_b,
                Gdk.Key.C or Gdk.Key.c => ScanCode.sc_c,
                Gdk.Key.D or Gdk.Key.d => ScanCode.sc_d,
                Gdk.Key.E or Gdk.Key.e => ScanCode.sc_e,
                Gdk.Key.F or Gdk.Key.f => ScanCode.sc_f,
                Gdk.Key.G or Gdk.Key.g => ScanCode.sc_g,
                Gdk.Key.H or Gdk.Key.h => ScanCode.sc_h,
                Gdk.Key.I or Gdk.Key.i => ScanCode.sc_i,
                Gdk.Key.J or Gdk.Key.j => ScanCode.sc_j,
                Gdk.Key.K or Gdk.Key.k => ScanCode.sc_k,
                Gdk.Key.L or Gdk.Key.l => ScanCode.sc_l,
                Gdk.Key.M or Gdk.Key.m => ScanCode.sc_m,
                Gdk.Key.N or Gdk.Key.n => ScanCode.sc_n,
                Gdk.Key.O or Gdk.Key.o => ScanCode.sc_o,
                Gdk.Key.P or Gdk.Key.p => ScanCode.sc_p,
                Gdk.Key.Q or Gdk.Key.q => ScanCode.sc_q,
                Gdk.Key.R or Gdk.Key.r => ScanCode.sc_r,
                Gdk.Key.S or Gdk.Key.s => ScanCode.sc_s,
                Gdk.Key.T or Gdk.Key.t => ScanCode.sc_t,
                Gdk.Key.U or Gdk.Key.u => ScanCode.sc_u,
                Gdk.Key.V or Gdk.Key.v => ScanCode.sc_v,
                Gdk.Key.W or Gdk.Key.w => ScanCode.sc_w,
                Gdk.Key.X or Gdk.Key.x => ScanCode.sc_x,
                Gdk.Key.Y or Gdk.Key.y => ScanCode.sc_y,
                Gdk.Key.Z or Gdk.Key.z => ScanCode.sc_z,
                Gdk.Key.Return or Gdk.Key.KP_Enter => ScanCode.sc_enter,
                Gdk.Key.Delete or Gdk.Key.BackSpace => ScanCode.sc_backspace,
                Gdk.Key.space => ScanCode.sc_space,
                Gdk.Key.comma => ScanCode.sc_comma,
                Gdk.Key.period or Gdk.Key.KP_Decimal => ScanCode.sc_period,
                Gdk.Key.semicolon => ScanCode.sc_semicolon,
                Gdk.Key.Escape => ScanCode.sc_escape,
                Gdk.Key.grave => ScanCode.sc_grave,
                Gdk.Key.apostrophe => ScanCode.sc_apostrophe,
                Gdk.Key.bracketleft => ScanCode.sc_bracketLeft,
                Gdk.Key.bracketright => ScanCode.sc_bracketRight,
                Gdk.Key.minus or Gdk.Key.KP_Subtract => ScanCode.sc_minus,
                Gdk.Key.plus or Gdk.Key.KP_Add => ScanCode.sc_equals,
                Gdk.Key.Tab => ScanCode.sc_tab,
                Gdk.Key.slash => ScanCode.sc_slash,
                Gdk.Key.backslash => ScanCode.sc_backslash,
                Gdk.Key.Shift_L or Gdk.Key.Shift_R => ScanCode.sc_shiftLeft,
                Gdk.Key.Menu or Gdk.Key.Alt_L or Gdk.Key.Alt_R => ScanCode.sc_altLeft,
                Gdk.Key.Control_L or Gdk.Key.Control_R => ScanCode.sc_controlLeft,
                Gdk.Key.Up or Gdk.Key.KP_Up => ScanCode.sc_up_arrow,
                Gdk.Key.Down or Gdk.Key.KP_Down => ScanCode.sc_down_arrow,
                Gdk.Key.Left or Gdk.Key.KP_Left => ScanCode.sc_left_arrow,
                Gdk.Key.Right or Gdk.Key.KP_Right => ScanCode.sc_right_arrow,
                Gdk.Key.F1 or Gdk.Key.F2 or Gdk.Key.F3 or
                Gdk.Key.F4 or Gdk.Key.F5 or Gdk.Key.F6 or
                Gdk.Key.F7 or Gdk.Key.F8 or Gdk.Key.F9 or
                Gdk.Key.F10 => ScanCode.sc_F1 + (key - Gdk.Key.F1),
                Gdk.Key.F11 or Gdk.Key.F12 => ScanCode.sc_F11 + (key - Gdk.Key.F11),
                _ => ScanCode.sc_null
            };
        }
    }
}
