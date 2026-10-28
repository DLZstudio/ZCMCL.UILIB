// SPDX-License-Identifier: Apache-2.0
// Copyright 2025 PCL Community
// 衍生自 PCL-Community/PCL-CE 的 PCL.Core（Apache-2.0）

namespace ZCMCL.UILIB.UI.Animation.ValueProcessor;

public class NScaleTransformValueProcessor : IValueProcessor<NScaleTransform>
{
    public NScaleTransform Filter(NScaleTransform value) => value;
    
    public NScaleTransform Add(NScaleTransform value1, NScaleTransform value2) => value1 + value2;

    public NScaleTransform Subtract(NScaleTransform value1, NScaleTransform value2) => value1 - value2;
    
    public NScaleTransform Scale(NScaleTransform value, double factor) => value * (float)factor;
    
    public NScaleTransform DefaultValue() => new();
    
    public bool Equal(NScaleTransform value1, NScaleTransform value2) => value1 == value2;
}